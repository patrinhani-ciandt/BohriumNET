using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class ScenarioUnitTestDO : BaseObjectDataDO
    {
        private List<string> _tags = new List<string>();

        [XmlIgnore]
        [ScriptIgnore]
        public MethodInfo TargetType { get; set; }
        
        [XmlAttribute]
        public Guid ParentFeature { get; set; }
        
        [XmlAttribute]
        public string Description { get; set; }

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public List<string> Tags
        {
            get { return _tags; }
        }

        [XmlArray("Statements")]
        [XmlArrayItem("Statement")]
        public List<GherkinBaseStatementDO> Statements { get; set; }
    }
}