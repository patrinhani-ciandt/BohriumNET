using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class BaseObjectDataDO
    {
        [XmlAttribute]
        public Guid ObjectId { get; set; }

        public BaseObjectDataDO()
        {
            ObjectId = Guid.NewGuid();
        }
    }
}