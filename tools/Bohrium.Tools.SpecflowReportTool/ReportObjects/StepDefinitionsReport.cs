using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bohrium.Tools.SpecflowReportTool.DataObjects;

namespace Bohrium.Tools.SpecflowReportTool.ReportObjects
{
    [Serializable]
    public class StepDefinitionsReport
    {
        [XmlArray("StepDefinitions")]
        [XmlArrayItem("StepDefinition")]
        public List<StepDefinitionDO> StepDefinitions { get; set; }

        public IEnumerable<BaseStepDefinitionTypeDO> FindBindableSteps(GherkinBaseStatementDO gherkinStatement)
        {
            return StepDefinitions
                .SelectMany(stepDefinitionDO => stepDefinitionDO.TestExpression(gherkinStatement));
        }
    }
}
