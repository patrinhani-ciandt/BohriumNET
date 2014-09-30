using System;
using System.Collections.Generic;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableDataDO
    {
        private List<SpecflowTableRowDataDO> _rows = new List<SpecflowTableRowDataDO>();

        public SpecflowTableHeaderDataDO Header { get; set; }

        public List<SpecflowTableRowDataDO> Rows
        {
            get { return _rows; }
        }
    }
}