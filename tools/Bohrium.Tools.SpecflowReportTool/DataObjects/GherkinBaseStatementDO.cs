using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class GherkinBaseStatementDO : BaseObjectDataDO
    {
        [XmlAttribute]
        public string Keyword { get; set; }
        [XmlAttribute]
        public string Statement { get; set; }

        public string MultilineTextParameter { get; set; }
        
        public SpecflowTableDataDO TableParameter { get; set; }

        public void FillFromMatch(Match matchRegex, ScenarioMethodSourceSyntaxParser methodSourceParser)
        {
            Keyword = methodSourceParser.ParseStringValue(matchRegex.Groups["keyword"].Value).Trim();

            Statement = matchRegex.Groups["statement"].Value.Trim();

            MultilineTextParameter =
                (!matchRegex.Groups["multilineTextArg"].Value.Trim()
                    .Equals("null", StringComparison.InvariantCultureIgnoreCase))
                    ? matchRegex.Groups["multilineTextArg"].Value.Trim()
                    : null;

            if (!matchRegex.Groups["tableArg"].Value.Trim()
                .Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                var tableDeclaration = methodSourceParser.ParseTableDeclaration(matchRegex.Groups["tableArg"].Value.Trim());

                TableParameter = new SpecflowTableDataDO();

                foreach (Match tableDecla in tableDeclaration)
                {
                    if (tableDecla.Success)
                    {
                        var tableRowCells = methodSourceParser.ParseTableRowCellValues(tableDecla.Value.Trim());

                        var specflowTableCellData = tableRowCells.Select(i => new SpecflowTableCellDataDO() { Value = i }).ToList();

                        if (tableDecla.Groups["varTableCreation"].Success)
                        {
                            TableParameter.Header = new SpecflowTableHeaderDataDO();

                            TableParameter.Header.Columns = specflowTableCellData;
                        }
                        else
                        {
                            var specflowTableRowData = new SpecflowTableRowDataDO();

                            specflowTableRowData.Cells = specflowTableCellData;

                            TableParameter.Rows.Add(specflowTableRowData);
                        }
                    }
                }
            }
        }
    }
}