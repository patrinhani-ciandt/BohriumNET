using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects.Warnings
{
    [Serializable]
    [XmlType("MultipleMatchStepDefinitionBindings")]
    public class MultipleMatchStepDefinitionBindingsWarningDO : WarningDO
    {
        private List<StepDefinitionReferenceDO> _stepDefinitionReference = new List<StepDefinitionReferenceDO>();

        [XmlArray("StepDefinitionReferences")]
        [XmlArrayItem("StepDefinitionReference")]
        public List<StepDefinitionReferenceDO> StepDefinitionReferences
        {
            get { return _stepDefinitionReference; }
            set { _stepDefinitionReference = value; }
        }

        public MultipleMatchStepDefinitionBindingsWarningDO()
            : this(@"Multiple Step Definition bindings found.")
        {

        }

        public MultipleMatchStepDefinitionBindingsWarningDO(string warningMessage)
            : base(warningMessage)
        {
            
        }
    }
}
