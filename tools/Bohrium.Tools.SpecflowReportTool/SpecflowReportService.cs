using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Bohrium.Tools.SpecflowReportTool.Utils;
using System.CodeDom.Compiler;
using ICSharpCode.Decompiler;
using ICSharpCode.ILSpy;
using Mono.Cecil;
using NUnit.Framework;

namespace Bohrium.Tools.SpecflowReportTool
{
    public class SpecflowReportService : IDisposable
    {
        private AssemblyLoader testAssemblyLoader;

        public SpecflowReportService()
        {
            testAssemblyLoader = new AssemblyLoader();
        }

        public void ReadTestAssembly(string inputAssembly)
        {
            var loadAssembly = testAssemblyLoader.LoadAssembly(inputAssembly);
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(loadAssembly.Location);

            var specflowUnitTests = getSpecflowFeatureUnitTests(loadAssembly, assemblyDefinition);

            var specflowTestScenarios = getScenarioUnitTestsFromFeatureTestFixture(specflowUnitTests, loadAssembly, assemblyDefinition);

            Console.WriteLine();
        }

        private IEnumerable<FeatureUnitTestClass> getSpecflowFeatureUnitTests(Assembly loadAssembly, AssemblyDefinition assemblyDefinition)
        {
            var featureUnitTests = new List<FeatureUnitTestClass>();

            var featureUnitTestTypes = loadAssembly.GetTypes()
                .Where(t => 
                    (t.GetCustomAttributes<TestFixtureAttribute>().Any()) 
                    && (t.GetCustomAttributes<GeneratedCodeAttribute>().Any(a => a.Tool == "TechTalk.SpecFlow")))
                .ToList();

            foreach (var featureUnitTestType in featureUnitTestTypes.AsParallel())
            {
                var featureUnitTestClass = new FeatureUnitTestClass();

                featureUnitTestClass.TargetType = featureUnitTestType;

                var descriptionAttribute = featureUnitTestType.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();

                featureUnitTestClass.Description = (descriptionAttribute != null)
                    ? descriptionAttribute.Description
                    : null;

                var categoryAttributes = featureUnitTestType.GetCustomAttributes<CategoryAttribute>();

                if (categoryAttributes != null)
                {
                    foreach (var categoryAttribute in categoryAttributes)
                    {
                        featureUnitTestClass.Tags.Add(categoryAttribute.Name);
                    }
                }

                featureUnitTests.Add(featureUnitTestClass);
            }

            return featureUnitTests;
        }

        private List<ScenarioUnitTestClass> getScenarioUnitTestsFromFeatureTestFixture(IEnumerable<FeatureUnitTestClass> featureUnitTestClasses, Assembly loadAssembly, AssemblyDefinition assemblyDefinition)
        {
            var methodScenarios = new List<ScenarioUnitTestClass>();

            foreach (var featureUnitTestClass in featureUnitTestClasses)
            {
                var featureUnitTestTypeDefinition = assemblyDefinition.MainModule.Types
                    .Where(t => t.FullName == featureUnitTestClass.TargetType.FullName)
                    .SingleOrDefault();

                var methodInfos = featureUnitTestClass.TargetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var methodScenario in methodInfos.Where(methodInfo => methodInfo.GetCustomAttributes<TestAttribute>().Any()))
                {
                    var scenarioUnitTestClass = new ScenarioUnitTestClass();

                    scenarioUnitTestClass.ParentFeature = featureUnitTestClass.ObjectID;
                    scenarioUnitTestClass.TargetType = methodScenario;

                    var descriptionAttribute = methodScenario.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();

                    scenarioUnitTestClass.Description = (descriptionAttribute != null)
                        ? descriptionAttribute.Description
                        : null;

                    var categoryAttributes = methodScenario.GetCustomAttributes<CategoryAttribute>();

                    if (categoryAttributes != null)
                    {
                        foreach (var categoryAttribute in categoryAttributes)
                        {
                            scenarioUnitTestClass.Tags.Add(categoryAttribute.Name);
                        }
                    }

                    var methodDefinition = featureUnitTestTypeDefinition.Methods
                        .Where(m => m.Name == methodScenario.Name)
                        .SingleOrDefault();

                    string methodBodySourceCode = getSourceCode(methodDefinition);

                    var scenarioMethodSourceSyntaxParser = new ScenarioMethodSourceSyntaxParser(methodBodySourceCode);

                    var matchesGiven = scenarioMethodSourceSyntaxParser.ParseGiven();
                    var matchesWhen = scenarioMethodSourceSyntaxParser.ParseWhen();
                    var matchesThen = scenarioMethodSourceSyntaxParser.ParseThen();

                    methodScenarios.Add(scenarioUnitTestClass);

                    Console.WriteLine();
                }
            }

            return methodScenarios;
        }

        public string getSourceCode(MethodDefinition methodDefinition)
        {
            try
            {
                var csharpLanguage = new CSharpLanguage();
                var textOutput = new PlainTextOutput();
                var decompilationOptions = new DecompilationOptions();
                decompilationOptions.FullDecompilation = true;
                csharpLanguage.DecompileMethod(methodDefinition, textOutput, decompilationOptions);

                return textOutput.ToString();
            }
            catch (Exception exception)
            {
                return ("Error in creating source code from IL: " + exception.Message);
            }
        }

        #region IDisposable Implementation

        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                testAssemblyLoader.Dispose();
                testAssemblyLoader = null;
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }

        #endregion IDisposable Implementation
    }

    public class ScenarioMethodSourceSyntaxParser
    {
        private const string GivenWhenThenParamsRegex = "(?<params>(?<description>[\"](.+)[\"])(,(?<multilineTextArg>(.+)))?(,(?<tableArg>(.+)))?(,(?<keyword>(.+)))?)";
        private const string GivenRegex = "Given[(]" + GivenWhenThenParamsRegex + "[)][;]";
        private const string WhenRegex = "When[(]" + GivenWhenThenParamsRegex + "[)][;]";
        private const string ThenRegex = "Then[(]" + GivenWhenThenParamsRegex + "[)][;]";
        private const string AndRegex = "And[(]" + GivenWhenThenParamsRegex + "[)][;]";
        private const string ScenarioMethodEndRegex = "ScenarioCleanup[(](.*)[)][;]";
        private const string CSharpEndOfStatement = "(([^;].|\\n)*[;])";

        private readonly string _methodSourceCode;

        public ScenarioMethodSourceSyntaxParser(string methodSourceCode)
        {
            _methodSourceCode = methodSourceCode;
        }

        private string createRegexStatement(string statementRegex)
        {
            return "(testRunner[.]" + statementRegex + ")";
        }

        private string createRegexStatementBlock(string beginStatement, string endStatement)
        {
            return "(testRunner[.]" + beginStatement + ")((.|\n)*(?=" + endStatement + "))";
        }

        private Match matchTableDeclaration(string tableVarName)
        {
            return Regex.Match(_methodSourceCode, "((?<varTableCreation>" + tableVarName + "\\s+=\\s+new\\s+TechTalk[.]SpecFlow[.]Table[(])|(?<varTableAddRow>" + tableVarName + ".AddRow(.)))" + CSharpEndOfStatement);
        }

        public MatchCollection ParseGiven()
        {
            var match = Regex.Match(_methodSourceCode, createRegexStatementBlock(GivenRegex, WhenRegex));

            string givenSourceBlock = match.Value;

            return Regex.Matches(givenSourceBlock, "(" + createRegexStatement(GivenRegex) + "|" + createRegexStatement(AndRegex) + ")");
        }

        public MatchCollection ParseWhen()
        {
            var match = Regex.Match(_methodSourceCode, createRegexStatementBlock(WhenRegex, ThenRegex));

            string givenSourceBlock = match.Value;

            return Regex.Matches(givenSourceBlock, "(" + createRegexStatement(WhenRegex) + "|" + createRegexStatement(AndRegex) + ")");
        }

        public MatchCollection ParseThen()
        {
            var match = Regex.Match(_methodSourceCode, createRegexStatementBlock(ThenRegex, ScenarioMethodEndRegex));

            string givenSourceBlock = match.Value;

            return Regex.Matches(givenSourceBlock, "(" + createRegexStatement(ThenRegex) + "|" + createRegexStatement(AndRegex) + ")");
        }
    }

    public class BaseObjectDataClass
    {
        public Guid ObjectID { get; set; }

        public BaseObjectDataClass()
        {
            ObjectID = Guid.NewGuid();
        }
    }

    public class FeatureUnitTestClass : BaseObjectDataClass
    {
        private IList<string> _tags = new List<string>();

        public Type TargetType { get; set; }

        public string Description { get; set; }

        public IList<string> Tags
        {
            get { return _tags; }
        }
    }

    public class ScenarioUnitTestClass : BaseObjectDataClass
    {
        private IList<string> _tags = new List<string>();

        public MethodInfo TargetType { get; set; }
        public Guid ParentFeature { get; set; }
        public string Description { get; set; }

        public IList<string> Tags
        {
            get { return _tags; }
        }
    }
}
