using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableCellDataDO
    {
        [XmlAttribute]
        public string Value { get; set; }
    }
}