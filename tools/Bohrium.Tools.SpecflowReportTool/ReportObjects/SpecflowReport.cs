using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bohrium.Tools.SpecflowReportTool.DataObjects;

namespace Bohrium.Tools.SpecflowReportTool.ReportObjects
{
    public class SpecflowReport
    {
        public FeaturesReport FeaturesReport { get; set; }
        public ScenariosReport ScenariosReport { get; set; }
        public StepDefinitionsReport StepDefinitionsReport { get; set; }

        public void MapStepDefinitionsUsage()
        {
            var scenariosStatements = ScenariosReport.Scenarios.SelectMany(a => a.Statements.OfType<GherkinBaseStatementDO>());

            foreach (var gherkinBaseStatementDo in scenariosStatements)
            {
               var bindableStepDefinitions = StepDefinitionsReport.FindBindableSteps(gherkinBaseStatementDo);

                Console.WriteLine();
            }
        }
    }
}
