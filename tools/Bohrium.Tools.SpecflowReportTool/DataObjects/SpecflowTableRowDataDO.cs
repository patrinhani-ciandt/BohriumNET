using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableRowDataDO : BaseObjectDataDO
    {
        private List<SpecflowTableRowCellDataDo> _cells = new List<SpecflowTableRowCellDataDo>();

        [XmlArray("Cells")]
        [XmlArrayItem("Cell")]
        public List<SpecflowTableRowCellDataDo> Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }
    }
}