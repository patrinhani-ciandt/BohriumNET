using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class FeatureUnitTestDO : BaseObjectDataDO
    {
        private List<string> _tags = new List<string>();

        [XmlIgnore]
        [ScriptIgnore]
        public Type TargetType { get; set; }
        
        [XmlAttribute]
        public string Description { get; set; }

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public List<string> Tags
        {
            get { return _tags; }
        }

        public BackgroundFeatureDO Background { get; set; }
    }
}