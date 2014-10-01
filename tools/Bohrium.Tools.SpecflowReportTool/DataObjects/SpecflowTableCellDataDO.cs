using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableCellDataDO : BaseObjectDataDO
    {
        [XmlAttribute]
        public string Value { get; set; }
    }

    [Serializable]
    public class SpecflowTableHeaderColumnDataDo : SpecflowTableCellDataDO
    {
    }

    [Serializable]
    public class SpecflowTableRowCellDataDo : SpecflowTableCellDataDO
    {
        [XmlAttribute]
        public Guid HeaderColumnId { get; set; }
    }
}