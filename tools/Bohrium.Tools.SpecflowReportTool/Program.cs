using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bohrium.Core.Extensions;
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

                var xmlFeatures = extractSpecflowReport.FeaturesReport.ToXml();

                var xmlScenarios = extractSpecflowReport.ScenariosReport.ToXml();

                Console.WriteLine();
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
