using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bohrium.Core.Extensions;
using Bohrium.Tools.SpecflowReportTool.Extensions;
using Bohrium.Tools.SpecflowReportTool.ReportObjects;

namespace Bohrium.Tools.SpecflowReportTool
{
    class Program
    {
        private const string regexForValidArgumentOperator = @"([/-](?<opr_name>\w+)([:](?<opr_param>\w+))?)";
        private const string regexFormatForSpecificOperatorName = @"([/-](?<opr_name>{0})([:](?<opr_param>\w+))?)";


        static void Main(string[] args)
        {
            processArgs(args);
        }

        private static void processArgs(string[] args)
        {
            string inputAssembly = getInputAssemblyArgument(args);

            if ((String.IsNullOrWhiteSpace(inputAssembly)) && (File.Exists(inputAssembly)))
                throw new ArgumentNullException("Input Assembly", "You need to set a valid Input Assembly to be analyzed as the fisrt argument.");

            using (var specflowRepostService = new SpecflowReportService())
            {
                specflowRepostService.ReadTestAssembly(inputAssembly);

                var extractSpecflowReport = specflowRepostService.ExtractSpecflowReport();

                string outputFolder = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "output");

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                string assemblyOutputFolder = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(inputAssembly));

                if (!Directory.Exists(assemblyOutputFolder))
                {
                    Directory.CreateDirectory(assemblyOutputFolder);
                }

                var xmlFeatures = extractSpecflowReport.FeaturesReport.ToXml();

                var xDocFeatures = XDocument.Parse(xmlFeatures);

                xDocFeatures.Save(Path.Combine(assemblyOutputFolder, "features-report.xml"));

                var jsonFeatures = extractSpecflowReport.FeaturesReport.ToJSon();

                saveToJSonFile(Path.Combine(assemblyOutputFolder, "features-report.json"), jsonFeatures);

                var xmlScenarios = extractSpecflowReport.ScenariosReport.ToXml();

                var xDocScenarios = XDocument.Parse(xmlScenarios);

                xDocScenarios.Save(Path.Combine(assemblyOutputFolder, "scenarios-report.xml"));

                var jsonScenarios = extractSpecflowReport.ScenariosReport.ToJSon();

                saveToJSonFile(Path.Combine(assemblyOutputFolder, "scenarios-report.json"), jsonScenarios);

                var xmlStepDefinitions = extractSpecflowReport.StepDefinitionsReport.ToXml();

                var xDocStepDefinitions = XDocument.Parse(xmlStepDefinitions);

                xDocStepDefinitions.Save(Path.Combine(assemblyOutputFolder, "stepdefinitions-report.xml"));

                var jsonStepDefinitions = extractSpecflowReport.StepDefinitionsReport.ToJSon();

                saveToJSonFile(Path.Combine(assemblyOutputFolder, "stepdefinitions-report.json"), jsonStepDefinitions);
            }
        }

        private static void saveToJSonFile(string outputFilePath, string jsonContent)
        {
            using (var jsonWriter = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                jsonWriter.Write(jsonContent);
            }
        }

        private static string getInputAssemblyArgument(string[] args)
        {
            if (args.Any() && (File.Exists(args.First())))
            {
                return args.First();
            }

            return null;
        }

        private static string getArgumentOperatorParameter(string argument)
        {
            string argParam = null;

            if (isValidInputForOperatorArgument(argument))
            {
                var match = Regex.Match(argument, regexForValidArgumentOperator);

                if (match.Success)
                {
                    if (match.Groups["opr_param"].Success)
                    {
                        argParam = match.Groups["opr_param"].Value;
                    }
                }
            }

            return argParam;
        }

        private static bool getValidInputsForOperatorArgument(string argument, string argOperator)
        {
            return Regex.IsMatch(argument, String.Format(regexFormatForSpecificOperatorName, argOperator));
        }

        private static bool hasArgument(string[] args, string argument)
        {
            return args.Any(a => getValidInputsForOperatorArgument(a, argument));
        }

        private static bool isValidInputForOperatorArgument(string argument)
        {
            return Regex.IsMatch(argument, regexForValidArgumentOperator);
        }
    }
}
