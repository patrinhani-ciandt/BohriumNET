using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class ScenarioUnitTestDO : BaseObjectDataDO
    {
        private List<string> _tags = new List<string>();

        [XmlIgnore]
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

        [XmlArray("GivenStatements")]
        [XmlArrayItem("Given")]
        public List<GivenStatementDO> GivenStatements { get; set; }

        [XmlArray("WhenStatements")]
        [XmlArrayItem("When")]
        public List<WhenStatementDO> WhenStatements { get; set; }

        [XmlArray("ThenStatements")]
        [XmlArrayItem("Then")]
        public List<ThenStatementDO> ThenStatements { get; set; }
    }
}