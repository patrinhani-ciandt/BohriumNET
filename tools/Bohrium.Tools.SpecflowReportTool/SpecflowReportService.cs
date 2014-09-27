using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var specflowUnitTests = getSpecflowFeatureUnitTests(loadAssembly);

            Console.WriteLine();
        }

        private IEnumerable<FeatureUnitTestClass> getSpecflowFeatureUnitTests(Assembly loadAssembly)
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

                var methodScenarios = getScenarioUnitTestsFromFeatureTestFixture(loadAssembly, featureUnitTestType);

                featureUnitTests.Add(featureUnitTestClass);
            }

            return featureUnitTests;
        }

        private List<MethodInfo> getScenarioUnitTestsFromFeatureTestFixture(Assembly loadAssembly, Type featureUnitTestTypes)
        {
            var methodScenarios = new List<MethodInfo>();

            var methodInfos =
                featureUnitTestTypes.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            methodScenarios.AddRange(methodInfos.Where(methodInfo => methodInfo.GetCustomAttributes<TestAttribute>().Any()));

            foreach (var methodScenario in methodScenarios)
            {
                var assemblyDefinition = AssemblyDefinition.ReadAssembly(loadAssembly.Location);

                var methodDefinition = assemblyDefinition.MainModule.Types
                    .Where(t => t.FullName == methodScenario.DeclaringType.FullName)
                    .SingleOrDefault()
                    .Methods.Where(m => m.Name == methodScenario.Name)
                    .SingleOrDefault();

                string methodBodySourceCode = getSourceCode(methodDefinition);

                Console.WriteLine();
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

                /*   ILanguage language = CSharp.GetLanguage(CSharpVersion.V1);
                   language.GetWriter(new PlainTextFormatter(writer)).Write(method);
                   MemoryStream stream = new MemoryStream();
                   StreamWriter writer3 = new StreamWriter(stream);
                   language.GetWriter(new PlainTextFormatter(writer3)).Write(method);
                   stream.Flush();*/

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

        public string Description { get; set; }

        public IList<string> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
    }
}
