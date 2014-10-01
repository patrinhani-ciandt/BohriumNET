using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableDataDO
    {
        private List<SpecflowTableRowDataDO> _rows = new List<SpecflowTableRowDataDO>();

        public SpecflowTableHeaderDataDO Header { get; set; }

        [XmlArray("Rows")]
        [XmlArrayItem("Row")]
        public List<SpecflowTableRowDataDO> Rows
        {
            get { return _rows; }
        }
    }
}