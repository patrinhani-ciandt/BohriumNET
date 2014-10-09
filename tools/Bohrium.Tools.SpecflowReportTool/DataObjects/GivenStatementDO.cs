using System;
using System.Xml.Serialization;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    [XmlType("Given")]
    public class GivenStatementDO : GherkinBaseStatementDO
    {
    }
}