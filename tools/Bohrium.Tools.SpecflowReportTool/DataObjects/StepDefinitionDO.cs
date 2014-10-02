using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using TechTalk.SpecFlow;

namespace Bohrium.Tools.SpecflowReportTool.DataObjects
{
    [Serializable]
    public class StepDefinitionDO : BaseObjectDataDO
    {
        private List<BaseStepDefinitionTypeDO> _stepDefinitionTypes = new List<BaseStepDefinitionTypeDO>();

        [XmlAttribute]
        public string StepDefinitionMethodName { get; set; }

        [XmlAttribute]
        public string StepDefinitionMethodSignature { get; set; }

        [XmlAttribute]
        public int CountUsages { get; set; }

        [XmlArray("StepDefinitionTypes")]
        [XmlArrayItem("StepDefinitionType")]
        public List<BaseStepDefinitionTypeDO> StepDefinitionTypes
        {
            get { return _stepDefinitionTypes; }
            set { _stepDefinitionTypes = value; }
        }

        public bool IsBindable(GherkinBaseStatementDO gherkinStatement)
        {
            return TestExpression(gherkinStatement).Any();
        }

        public IEnumerable<BaseStepDefinitionTypeDO> TestExpression(GherkinBaseStatementDO gherkinStatement)
        {
            foreach (var stepDefinitionType in StepDefinitionTypes
                .Where(a => a.GetType() == gherkinStatement.GetTypeOfBaseStepDefinitionTypeDO())
                .Select(a => a)
                .AsParallel())
            {
                if (Regex.IsMatch(gherkinStatement.Statement, @"\b" + stepDefinitionType.Attribute.Regex + @"\b$"))
                    yield return stepDefinitionType;
            }
        }

        public void PlusOneUsage()
        {
            CountUsages += 1;
        }
    }

    public static class StepDefinitionTypeDOUtils
    {
        public static Type GetTypeOfBaseStepDefinitionTypeDO(this GherkinBaseStatementDO gherkinStatement)
        {
            if (gherkinStatement is GivenStatementDO)
                return typeof (GivenStepDefinitionTypeDO);
            if (gherkinStatement is WhenStatementDO)
                return typeof(WhenStepDefinitionTypeDO);
            if (gherkinStatement is ThenStatementDO)
                return typeof(ThenStepDefinitionTypeDO);

            return null;
        }

        public static BaseStepDefinitionTypeDO GetStepDefinitionTypeDO(this StepDefinitionBaseAttribute stepDefinitionBaseAttribute)
        {
            BaseStepDefinitionTypeDO stepDefinitionTypeDO = null;

            if (stepDefinitionBaseAttribute is GivenAttribute)
                stepDefinitionTypeDO = new GivenStepDefinitionTypeDO();
            if (stepDefinitionBaseAttribute is WhenAttribute)
                stepDefinitionTypeDO = new WhenStepDefinitionTypeDO();
            if (stepDefinitionBaseAttribute is ThenAttribute)
                stepDefinitionTypeDO = new ThenStepDefinitionTypeDO();

            if (stepDefinitionTypeDO != null)
            {
                stepDefinitionTypeDO.RegexExpression = stepDefinitionBaseAttribute.Regex;
                stepDefinitionTypeDO.Attribute = stepDefinitionBaseAttribute;
            }

            return stepDefinitionTypeDO;
        }
    }

    [Serializable]
    [XmlInclude(typeof(GivenStepDefinitionTypeDO))]
    [XmlInclude(typeof(WhenStepDefinitionTypeDO))]
    [XmlInclude(typeof(ThenStepDefinitionTypeDO))]
    public abstract class BaseStepDefinitionTypeDO : BaseObjectDataDO
    {
        [XmlIgnore]
        [ScriptIgnore]
        public StepDefinitionBaseAttribute Attribute { get; set; }

        [XmlIgnore]
        [ScriptIgnore]
        public StepDefinitionDO ParentStepDefinition { get; set; }

        [XmlAttribute]
        public Guid ParentStepDefinitionId { get; set; }

        [XmlAttribute]
        public string RegexExpression { get; set; }

        [XmlAttribute]
        public int CountUsages { get; set; }

        public void PlusOneUsage()
        {
            CountUsages += 1;
        }
    }

    [Serializable]
    [XmlType("Given")]
    public class GivenStepDefinitionTypeDO : BaseStepDefinitionTypeDO
    {
    }

    [Serializable]
    [XmlType("When")]
    public class WhenStepDefinitionTypeDO : BaseStepDefinitionTypeDO
    {
    }

    [Serializable]
    [XmlType("Then")]
    public class ThenStepDefinitionTypeDO : BaseStepDefinitionTypeDO
    {
    }
}
