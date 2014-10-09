using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bohrium.Tools.SpecflowReportTool.Parsers
{
    public class ScenarioMethodSourceSyntaxParser
    {
        private const string StringValueRegex = "(([\"])(?<value>([^,\"]|[.])+)(([^\"].|\n)*[\"]))";

        private const string GivenWhenThenParamDescriptionRegex = "([\"](?<statement>([^,]|[.])+)[\"])";
        private const string GivenWhenThenParamMultilineTextArgRegex = "([,](?<multilineTextArg>([^,]|[.])+))?";
        private const string GivenWhenThenParamTableArgRegex = "([,](?<tableArg>([^,]|[.])+))?";
        private const string GivenWhenThenParamKeywordRegex = "([,](?<keyword>([^,]|[.])+))?";
        private const string GivenWhenThenParamsRegex = "("
                                                        + GivenWhenThenParamDescriptionRegex
                                                        + GivenWhenThenParamMultilineTextArgRegex
                                                        + GivenWhenThenParamTableArgRegex
                                                        + GivenWhenThenParamKeywordRegex + ")";
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
            return "([.]" + statementRegex + ")";
        }

        private string createRegexStatementBlock(string beginStatement, string endStatement, string intermadiateNeg = "")
        {
            string regexIntermadiateNeg = (!string.IsNullOrWhiteSpace(intermadiateNeg)) ? "(?!" + intermadiateNeg + ")|" : String.Empty;

            return "([.]" + beginStatement + ")((" + regexIntermadiateNeg + ".|\n)*(?=" + endStatement + "))";
        }

        public MatchCollection ParseTableDeclaration(string tableVarName)
        {
            return Regex.Matches(_methodSourceCode, "((?<varTableCreation>" + tableVarName + @"\s+=\s+new\s+(TechTalk[.]SpecFlow[.])?Table[(](.*))|(?<varTableAddRow>" + tableVarName + "[.]AddRow(.*)))" + CSharpEndOfStatement, RegexOptions.Multiline);
        }

        public MatchCollection ParseTableRows(string tableDeclarationSource)
        {
            return Regex.Matches(tableDeclarationSource, "([{])(.*)(([^);].|\n)*[}])" + CSharpEndOfStatement, RegexOptions.Multiline);
        }

        public IEnumerable<string> ParseTableRowCellValues(string tableRowSource)
        {
            IList<string> cellValues = new List<string>();

            var matchCollection = Regex.Matches(tableRowSource, "([\"])(?<column>[^,\"]*)(([^\"].|\n)*[\"])", RegexOptions.Multiline | RegexOptions.ExplicitCapture);

            foreach (Match match in matchCollection)
            {
                if (match.Groups["column"].Success)
                {
                    cellValues.Add(match.Groups["column"].Value.Trim());
                }
            }

            return cellValues;
        }

        public MatchCollection ParseGroupStatements()
        {
            var regexGiven = "(?<given>" + createRegexStatementBlock(GivenRegex, "[.](When|Then|ScenarioCleanup)", "Given") + ")";
            var regexWhen = "(?<when>" + createRegexStatementBlock(WhenRegex, "[.](Given|Then|ScenarioCleanup)", "When") + ")";
            var regexThen = "(?<then>" + createRegexStatementBlock(ThenRegex, "[.](Given|When|ScenarioCleanup)", "Then") + ")";

            return Regex.Matches(_methodSourceCode, regexGiven + "|" + regexWhen + "|" + regexThen, RegexOptions.ExplicitCapture);
        }

        public MatchCollection ParseGroupGiven(string givenSourceGroup)
        {
            return Regex.Matches(givenSourceGroup, "(" + createRegexStatement(GivenRegex) + "|" + createRegexStatement(AndRegex) + ")", RegexOptions.ExplicitCapture);
        }

        public MatchCollection ParseGroupWhen(string whenSourceGroup)
        {
            return Regex.Matches(whenSourceGroup, "(" + createRegexStatement(WhenRegex) + "|" + createRegexStatement(AndRegex) + ")", RegexOptions.ExplicitCapture);
        }

        public MatchCollection ParseGroupThen(string thenSourceGroup)
        {
            return Regex.Matches(thenSourceGroup, "(" + createRegexStatement(ThenRegex) + "|" + createRegexStatement(AndRegex) + ")", RegexOptions.ExplicitCapture);
        }

        public string ParseStringValue(string strDeclaration)
        {
            var match = Regex.Match(strDeclaration, StringValueRegex);

            return match.Groups["value"].Value;
        }
    }
}