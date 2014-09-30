using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class BackgroundFeatureDO : BaseObjectDataDO
    {
        [XmlAttribute]
        public Guid ParentFeature { get; set; }
    }
}