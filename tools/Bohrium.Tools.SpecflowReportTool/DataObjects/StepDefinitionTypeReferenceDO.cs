using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class StepDefinitionTypeReferenceDO
    {
        [XmlAttribute]
        public Guid StepDefinitionTypeId { get; set; }
    }
}
