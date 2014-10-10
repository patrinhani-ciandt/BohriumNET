using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bohrium.Tools.SpecflowReportTool.DataObjects;
using Bohrium.Tools.SpecflowReportTool.DataObjects.Warnings;

namespace Bohrium.Tools.SpecflowReportTool.ReportObjects
{
    public class SpecflowReport
    {
        public FeaturesReport FeaturesReport { get; set; }
        public ScenariosReport ScenariosReport { get; set; }
        public StepDefinitionsReport StepDefinitionsReport { get; set; }

        public void MapStepDefinitionsUsage()
        {
            Console.Write("Mapping StepDefinition usages ... ");

            var featuresBackgroundStatements = FeaturesReport.Features.SelectMany(a => a.Background.Statements.OfType<GherkinBaseStatementDO>());

            mapStepDefinitionsUsage(featuresBackgroundStatements);

            var scenariosStatements = ScenariosReport.Scenarios.SelectMany(a => a.Statements.OfType<GherkinBaseStatementDO>());

            mapStepDefinitionsUsage(scenariosStatements);

            Console.WriteLine("[DONE]");
        }

        private void mapStepDefinitionsUsage(IEnumerable<GherkinBaseStatementDO> scenariosStatements)
        {
            foreach (var gherkinBaseStatementDo in scenariosStatements.AsParallel())
            {
                var bindableStepDefinitions = StepDefinitionsReport.FindBindableSteps(gherkinBaseStatementDo);

                bool hasMultipleStepDefinitionBindings =
                    (bindableStepDefinitions.Select(x => x.ParentStepDefinitionId).Distinct().Count() > 1);

                var firstStepDefinitionTypeGroupBinding =
                    bindableStepDefinitions.GroupBy(x => x.ParentStepDefinitionId).FirstOrDefault();

                if (firstStepDefinitionTypeGroupBinding != null)
                {
                    gherkinBaseStatementDo.StatementStepDefinitionReference = new StepDefinitionReferenceDO()
                    {
                        StepDefinitionId = firstStepDefinitionTypeGroupBinding.Key
                    };

                    var baseRefstepDefinitionType = firstStepDefinitionTypeGroupBinding.FirstOrDefault();

                    if (baseRefstepDefinitionType != null)
                    {
                        var parentStepDefinition = baseRefstepDefinitionType.ParentStepDefinition;

                        foreach (var baseStepDefinitionTypeDO in firstStepDefinitionTypeGroupBinding)
                        {
                            var stepDefinitionTypeReferenceDO = new StepDefinitionTypeReferenceDO()
                            {
                                StepDefinitionTypeId = baseStepDefinitionTypeDO.ObjectId
                            };

                            gherkinBaseStatementDo.StatementStepDefinitionReference.StepDefinitionTypeReferences.Add(
                                stepDefinitionTypeReferenceDO);

                            baseStepDefinitionTypeDO.PlusOneUsage();
                        }

                        parentStepDefinition.PlusOneUsage();
                    }
                }

                if (hasMultipleStepDefinitionBindings)
                {
                    gherkinBaseStatementDo.Warnings = new List<WarningDO>();

                    var warningDO = new MultipleMatchStepDefinitionBindingsWarningDO();

                    warningDO.StepDefinitionReferences = bindableStepDefinitions
                        .GroupBy(x => x.ParentStepDefinitionId)
                        .Select(x => x.Key)
                        .Distinct()
                        .Select(x => new StepDefinitionReferenceDO {StepDefinitionId = x, StepDefinitionTypeReferences = null})
                        .ToList();

                    gherkinBaseStatementDo.Warnings.Add(warningDO);
                }
            }
        }
    }
}
