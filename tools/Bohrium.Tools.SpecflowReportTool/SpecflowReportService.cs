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

            return specflowReport;
        }

        public FeaturesReport ExtractFeaturesReport()
        {
            chkLoadedAssembly();

            var specflowUnitTests = getSpecflowFeatureUnitTests(loadAssembly);

            var featuresReport = new FeaturesReport();

            featuresReport.Features = specflowUnitTests.ToList();

            return featuresReport;
        }

        public ScenariosReport ExtractScenariosReport(FeaturesReport featuresReport)
        {
            chkLoadedAssembly();

            if (featuresReport == null) throw new ArgumentNullException("featuresReport");

            var specflowTestScenarios = getScenarioUnitTestsFromFeatureTestFixture(featuresReport.Features, assemblyDefinition);

            var scenariosReport = new ScenariosReport();

            scenariosReport.Scenarios = specflowTestScenarios.ToList();

            return scenariosReport;
        }

        private IEnumerable<FeatureUnitTestDO> getSpecflowFeatureUnitTests(Assembly loadAssembly)
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

        private List<ScenarioUnitTestDO> getScenarioUnitTestsFromFeatureTestFixture(IEnumerable<FeatureUnitTestDO> featureUnitTestClasses, AssemblyDefinition assemblyDefinition)
        {
            var methodScenarios = new List<ScenarioUnitTestDO>();

            foreach (var featureUnitTestClass in featureUnitTestClasses)
            {
                var featureUnitTestTypeDefinition = assemblyDefinition.MainModule.Types
                    .Where(t => t.FullName == featureUnitTestClass.TargetType.FullName)
                    .SingleOrDefault();

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
                        .Where(m => m.Name == methodScenario.Name)
                        .SingleOrDefault();

                    string methodBodySourceCode = methodDefinition.GetSourceCode();

                    IList<GivenStatementDO> givenStatements;
                    IList<WhenStatementDO> whenStatements;
                    IList<ThenStatementDO> thenStatements;

                    parseGivenWhenThenStatements(methodBodySourceCode, out givenStatements, out whenStatements, out thenStatements);

                    scenarioUnitTestClass.GivenStatements = givenStatements.ToList();
                    scenarioUnitTestClass.WhenStatements = whenStatements.ToList();
                    scenarioUnitTestClass.ThenStatements = thenStatements.ToList();

                    methodScenarios.Add(scenarioUnitTestClass);
                }
            }

            return methodScenarios;
        }

        private void parseGivenWhenThenStatements(string methodBodySourceCode,
            out IList<GivenStatementDO> givenStatements,
            out IList<WhenStatementDO> whenStatements,
            out IList<ThenStatementDO> thenStatements)
        {
            givenStatements = new List<GivenStatementDO>();
            whenStatements = new List<WhenStatementDO>();
            thenStatements = new List<ThenStatementDO>();

            var scenarioMethodSourceSyntaxParser = new ScenarioMethodSourceSyntaxParser(methodBodySourceCode);

            var matchesGiven = scenarioMethodSourceSyntaxParser.ParseGiven();
            var matchesWhen = scenarioMethodSourceSyntaxParser.ParseWhen();
            var matchesThen = scenarioMethodSourceSyntaxParser.ParseThen();

            foreach (Match givenMatch in matchesGiven)
            {
                if (givenMatch.Success)
                {
                    var givenStatementClass = new GivenStatementDO();

                    givenStatementClass.FillFromMatch(givenMatch, scenarioMethodSourceSyntaxParser);

                    givenStatements.Add(givenStatementClass);
                }
            }

            foreach (Match whenMatch in matchesWhen)
            {
                if (whenMatch.Success)
                {
                    var whenStatementClass = new WhenStatementDO();

                    whenStatementClass.FillFromMatch(whenMatch, scenarioMethodSourceSyntaxParser);

                    whenStatements.Add(whenStatementClass);
                }
            }

            foreach (Match thenMatch in matchesThen)
            {
                if (thenMatch.Success)
                {
                    var thenStatementClass = new ThenStatementDO();

                    thenStatementClass.FillFromMatch(thenMatch, scenarioMethodSourceSyntaxParser);

                    thenStatements.Add(thenStatementClass);
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
