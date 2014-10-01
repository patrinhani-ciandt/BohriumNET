using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    [XmlType("When")]
    public class WhenStatementDO : GherkinBaseStatementDO
    {
    }
}