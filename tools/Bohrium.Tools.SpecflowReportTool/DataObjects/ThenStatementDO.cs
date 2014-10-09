using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    [XmlType("Then")]
    public class ThenStatementDO : GherkinBaseStatementDO
    {
    }
}