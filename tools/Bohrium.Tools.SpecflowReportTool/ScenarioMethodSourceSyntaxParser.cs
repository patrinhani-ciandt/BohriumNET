using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bohrium.Tools.SpecflowReportTool
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
            return "(testRunner[.]" + statementRegex + ")";
        }

        private string createRegexStatementBlock(string beginStatement, string endStatement)
        {
            return "(testRunner[.]" + beginStatement + ")((.|\n)*(?=" + endStatement + "))";
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

        public MatchCollection ParseGiven()
        {
            var match = Regex.Match(_methodSourceCode, createRegexStatementBlock(GivenRegex, WhenRegex));

            string givenSourceBlock = match.Value;

            return Regex.Matches(givenSourceBlock, "(" + createRegexStatement(GivenRegex) + "|" + createRegexStatement(AndRegex) + ")", RegexOptions.ExplicitCapture);
        }

        public MatchCollection ParseWhen()
        {
            var match = Regex.Match(_methodSourceCode, createRegexStatementBlock(WhenRegex, ThenRegex));

            string givenSourceBlock = match.Value;

            return Regex.Matches(givenSourceBlock, "(" + createRegexStatement(WhenRegex) + "|" + createRegexStatement(AndRegex) + ")", RegexOptions.ExplicitCapture);
        }

        public MatchCollection ParseThen()
        {
            var match = Regex.Match(_methodSourceCode, createRegexStatementBlock(ThenRegex, ScenarioMethodEndRegex));

            string givenSourceBlock = match.Value;

            return Regex.Matches(givenSourceBlock, "(" + createRegexStatement(ThenRegex) + "|" + createRegexStatement(AndRegex) + ")", RegexOptions.ExplicitCapture);
        }

        public string ParseStringValue(string strDeclaration)
        {
            var match = Regex.Match(strDeclaration, StringValueRegex);

            return match.Groups["value"].Value;
        }

    }
}