using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Bohrium.Core.Extensions;
using Bohrium.Tools.SpecflowReportTool.DataObjects;
using Bohrium.Tools.SpecflowReportTool.Extensions;
using Bohrium.Tools.SpecflowReportTool.ReportObjects;
using Bohrium.Tools.SpecflowReportTool.Utils;
using System.CodeDom.Compiler;
using Mono.Cecil;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Bohrium.Tools.SpecflowReportTool
{
    public class SpecflowReportService : IDisposable
    {
        private AssemblyLoader testAssemblyLoader;
        private Assembly loadAssembly;
        private AssemblyDefinition assemblyDefinition;

        public SpecflowReportService()
        {
            testAssemblyLoader = new AssemblyLoader();
        }

        public void ReadTestAssembly(string inputAssembly)
        {
            this.loadAssembly = testAssemblyLoader.LoadAssembly(inputAssembly);
            this.assemblyDefinition = AssemblyDefinition.ReadAssembly(loadAssembly.Location);
        }

        private void chkLoadedAssembly()
        {
            if ((loadAssembly == null) || (assemblyDefinition == null))
                throw new InvalidOperationException("The Test assembly need to be loaded before running the method 'ReadTestAssembly'.");
        }

        public SpecflowReport ExtractSpecflowReport()
        {
            var specflowReport = new SpecflowReport();

            specflowReport.FeaturesReport = ExtractFeaturesReport();

            specflowReport.ScenariosReport = ExtractScenariosReport(specflowReport.FeaturesReport);

            specflowReport.StepDefinitionsReport = ExtractStepDefinitionsReport();

            specflowReport.MapStepDefinitionsUsage();

            return specflowReport;
        }

        public FeaturesReport ExtractFeaturesReport()
        {
            chkLoadedAssembly();

            var specflowUnitTests = getSpecflowFeatureUnitTests();

            var featuresReport = new FeaturesReport();

            featuresReport.Features = specflowUnitTests.ToList();

            return featuresReport;
        }

        public ScenariosReport ExtractScenariosReport(FeaturesReport featuresReport)
        {
            chkLoadedAssembly();

            if (featuresReport == null) throw new ArgumentNullException("featuresReport");

            var specflowTestScenarios = getScenarioUnitTestsFromFeatureTestFixture(featuresReport.Features);

            var scenariosReport = new ScenariosReport();

            scenariosReport.Scenarios = specflowTestScenarios.ToList();

            return scenariosReport;
        }

        public StepDefinitionsReport ExtractStepDefinitionsReport()
        {
            chkLoadedAssembly();

            var specflowStepDefinitions = getStepDefinitions();

            var scenariosReport = new StepDefinitionsReport();

            scenariosReport.StepDefinitions = specflowStepDefinitions.ToList();

            return scenariosReport;
        }

        private IEnumerable<StepDefinitionDO> getStepDefinitions()
        {
            var stepDefinitions = new List<StepDefinitionDO>();

            var stepDefinitionsBindTypes = loadAssembly.GetTypes()
                .Where(t => t.GetCustomAttributes<BindingAttribute>().Any())
                .ToList();

            foreach (var stepDefinitionsBindType in stepDefinitionsBindTypes)
            {
                var stepDefinitionsBindTypeDefinition = assemblyDefinition.MainModule.Types
                    .SingleOrDefault(t => t.FullName == stepDefinitionsBindType.FullName);

                var stepDefinitionMethodInfos = stepDefinitionsBindType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(methodInfo => methodInfo.GetCustomAttributes<StepDefinitionBaseAttribute>().Any());

                foreach (var stepDefinitionMethodInfo in stepDefinitionMethodInfos)
                {
                    var methodDefinition = stepDefinitionsBindTypeDefinition.Methods
                        .SingleOrDefault(m => m.Name == stepDefinitionMethodInfo.Name);

                    var stepDefinitionDO = new StepDefinitionDO();

                    stepDefinitionDO.StepDefinitionMethodName = stepDefinitionMethodInfo.Name;

                    var stepDefinitionBaseAttributes = stepDefinitionMethodInfo.GetCustomAttributes<StepDefinitionBaseAttribute>();

                    foreach (var stepDefinitionBaseAttribute in stepDefinitionBaseAttributes)
                    {
                        stepDefinitionDO.StepDefinitionTypes.Add(stepDefinitionBaseAttribute.GetStepDefinitionTypeDO());
                    }

                    stepDefinitions.Add(stepDefinitionDO);
                }
            }

            return stepDefinitions;
        }

        private IEnumerable<FeatureUnitTestDO> getSpecflowFeatureUnitTests()
        {
            var featureUnitTests = new List<FeatureUnitTestDO>();

            var featureUnitTestTypes = loadAssembly.GetTypes()
                .Where(t =>
                    (t.GetCustomAttributes<TestFixtureAttribute>().Any())
                    && (t.GetCustomAttributes<GeneratedCodeAttribute>().Any(a => a.Tool == "TechTalk.SpecFlow")))
                .ToList();

            foreach (var featureUnitTestType in featureUnitTestTypes.AsParallel())
            {
                var featureUnitTestClass = new FeatureUnitTestDO();

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

        private List<ScenarioUnitTestDO> getScenarioUnitTestsFromFeatureTestFixture(IEnumerable<FeatureUnitTestDO> featureUnitTestClasses)
        {
            var methodScenarios = new List<ScenarioUnitTestDO>();

            foreach (var featureUnitTestClass in featureUnitTestClasses)
            {
                var featureUnitTestTypeDefinition = assemblyDefinition.MainModule.Types
                    .SingleOrDefault(t => t.FullName == featureUnitTestClass.TargetType.FullName);

                var methodInfos = featureUnitTestClass.TargetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var methodScenario in methodInfos.Where(methodInfo => methodInfo.GetCustomAttributes<TestAttribute>().Any()))
                {
                    var scenarioUnitTestClass = new ScenarioUnitTestDO();

                    scenarioUnitTestClass.ParentFeature = featureUnitTestClass.ObjectId;
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
                        .SingleOrDefault(m => m.Name == methodScenario.Name);

                    string methodBodySourceCode = methodDefinition.GetSourceCode();

                    IList<GherkinBaseStatementDO> statements;

                    parseGivenWhenThenStatements(methodBodySourceCode, out statements);

                    scenarioUnitTestClass.Statements = statements.ToList();

                    methodScenarios.Add(scenarioUnitTestClass);
                }
            }

            return methodScenarios;
        }

        private void parseGivenWhenThenStatements(string methodBodySourceCode,
            out IList<GherkinBaseStatementDO> statements)
        {
            statements = new List<GherkinBaseStatementDO>();

            var scenarioMethodSourceSyntaxParser = new ScenarioMethodSourceSyntaxParser(methodBodySourceCode);

            var matchGroupStatements = scenarioMethodSourceSyntaxParser.ParseGroupStatements();

            foreach (Match matchGroupStatement in matchGroupStatements)
            {
                if (matchGroupStatement.Groups["given"].Success)
                {
                    var matchesGiven = scenarioMethodSourceSyntaxParser.ParseGroupGiven(matchGroupStatement.Groups["given"].Value);
                    foreach (Match givenMatch in matchesGiven)
                    {
                        if (givenMatch.Success)
                        {
                            var givenStatementClass = new GivenStatementDO();

                            givenStatementClass.FillFromMatch(givenMatch, scenarioMethodSourceSyntaxParser);

                            statements.Add(givenStatementClass);
                        }
                    }
                }
                else if (matchGroupStatement.Groups["when"].Success)
                {
                    var matchesWhen = scenarioMethodSourceSyntaxParser.ParseGroupWhen(matchGroupStatement.Groups["when"].Value);
                    foreach (Match whenMatch in matchesWhen)
                    {
                        if (whenMatch.Success)
                        {
                            var whenStatementClass = new WhenStatementDO();

                            whenStatementClass.FillFromMatch(whenMatch, scenarioMethodSourceSyntaxParser);

                            statements.Add(whenStatementClass);
                        }
                    }
                }
                else if (matchGroupStatement.Groups["then"].Success)
                {
                    var matchesThen = scenarioMethodSourceSyntaxParser.ParseGroupThen(matchGroupStatement.Groups["then"].Value);
                    foreach (Match thenMatch in matchesThen)
                    {
                        if (thenMatch.Success)
                        {
                            var thenStatementClass = new ThenStatementDO();

                            thenStatementClass.FillFromMatch(thenMatch, scenarioMethodSourceSyntaxParser);

                            statements.Add(thenStatementClass);
                        }
                    }
                }
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
}
