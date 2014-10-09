using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class BackgroundFeatureDO : BaseStatementContainerDO
    {
        [XmlAttribute]
        public Guid ParentFeature { get; set; }
    }
}