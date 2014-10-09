using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bohrium.Tools.SpecflowReportTool.DataObjects;
using Mono.Collections.Generic;

namespace Bohrium.Tools.SpecflowReportTool.ReportObjects
{
    [Serializable]
    public class FeaturesReport
    {
        [XmlArray("Features")]
        [XmlArrayItem("Feature")]
        public List<FeatureUnitTestDO> Features { get; set; }
    }
}
