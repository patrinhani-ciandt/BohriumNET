using System;
using System.Collections.Generic;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableHeaderDataDO
    {
        private List<SpecflowTableCellDataDO> _columns = new List<SpecflowTableCellDataDO>();

        public List<SpecflowTableCellDataDO> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }
    }
}