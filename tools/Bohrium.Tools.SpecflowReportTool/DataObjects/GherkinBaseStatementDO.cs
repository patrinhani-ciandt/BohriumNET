using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Bohrium.Tools.SpecflowReportTool.DataObjects.Warnings;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    [XmlInclude(typeof(GivenStatementDO))]
    [XmlInclude(typeof(WhenStatementDO))]
    [XmlInclude(typeof(ThenStatementDO))]
    public abstract class GherkinBaseStatementDO : BaseObjectDataDO
    {
        private List<WarningDO> _warnings = null;

        [XmlAttribute]
        public string Keyword { get; set; }
        [XmlAttribute]
        public string Statement { get; set; }

        public string MultilineTextParameter { get; set; }
        
        public SpecflowTableDataDO TableParameter { get; set; }

        [XmlElement("StepDefinitionReference")]
        public StepDefinitionReferenceDO StatementStepDefinitionReference { get; set; }

        [XmlArray("Warnings")]
        [XmlArrayItem("Warning")]
        public List<WarningDO> Warnings
        {
            get { return _warnings; }
            set { _warnings = value; }
        }

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
                        var tableRowCells = methodSourceParser.ParseTableRowCellValues(tableDecla.Value.Trim()).ToList();

                        if (tableDecla.Groups["varTableCreation"].Success)
                        {
                            TableParameter.Header = new SpecflowTableHeaderDataDO();

                            var specflowTableCellData = tableRowCells.Select(i => new SpecflowTableHeaderColumnDataDo() { Value = i }).ToList();

                            TableParameter.Header.Columns = specflowTableCellData;
                        }
                        else
                        {
                            var specflowTableRowData = new SpecflowTableRowDataDO();

                            for (int i = 0; i < tableRowCells.Count(); i++)
                            {
                                var tableHeaderColumnData = TableParameter.Header.Columns.Skip(i).FirstOrDefault();

                                var tableRowCellValue = tableRowCells[i];

                                var specflowTableRowCellDataDo = new SpecflowTableRowCellDataDo()
                                {
                                    HeaderColumnId = (tableHeaderColumnData != null) ? tableHeaderColumnData.ObjectId : Guid.Empty,
                                    Value = tableRowCellValue 
                                };

                                specflowTableRowData.Cells.Add(specflowTableRowCellDataDo);
                            }

                            TableParameter.Rows.Add(specflowTableRowData);
                        }
                    }
                }
            }
        }
    }
}