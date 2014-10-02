using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class StepDefinitionReferenceDO
    {
        private List<StepDefinitionTypeReferenceDO> _stepDefinitionTypeReferences = new List<StepDefinitionTypeReferenceDO>();

        [XmlAttribute]
        public Guid StepDefinitionId { get; set; }

        [XmlArray("TypeReferences")]
        [XmlArrayItem("StepDefinitionTypeReference")]
        public List<StepDefinitionTypeReferenceDO> StepDefinitionTypeReferences
        {
            get { return _stepDefinitionTypeReferences; }
            set { _stepDefinitionTypeReferences = value; }
        }
    }
}
