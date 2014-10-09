using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects.Warnings
{
    [Serializable]
    [XmlInclude(typeof(MultipleMatchStepDefinitionBindingsWarningDO))]
    public class WarningDO
    {
        [XmlAttribute]
        public string Message { get; set; }

        public WarningDO()
            : this(String.Empty)
        {
        }

        protected WarningDO(string warningMessage)
        {
            Message = warningMessage;
        }
    }
}
