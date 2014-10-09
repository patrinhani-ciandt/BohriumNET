using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    public class BaseStatementContainerDO : BaseObjectDataDO
    {
        [XmlArray("Statements")]
        [XmlArrayItem("Statement")]
        public List<GherkinBaseStatementDO> Statements { get; set; }

    }
}
