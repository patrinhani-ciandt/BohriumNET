using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class SpecflowTableHeaderDataDO
    {
        private List<SpecflowTableHeaderColumnDataDo> _columns = new List<SpecflowTableHeaderColumnDataDo>();

        [XmlArray("Columns")]
        [XmlArrayItem("Column")]
        public List<SpecflowTableHeaderColumnDataDo> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }
    }
}