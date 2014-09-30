using System;
using System.Collections.Generic;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableRowDataDO
    {
        private List<SpecflowTableCellDataDO> _cells = new List<SpecflowTableCellDataDO>();

        public List<SpecflowTableCellDataDO> Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }
    }
}