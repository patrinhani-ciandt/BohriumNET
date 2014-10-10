using TechTalk.SpecFlow;

namespace Bohrium.Tools.Sample.SpecflowTestsBase.StepDefinitions
{
    [Binding]
    public class StepDefinitions001
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        [Given(@"I am logged as SA")]
        [When(@"I am logged as SA")]
        public void GivenIAmLoggedAsSA()
        {
            //ScenarioContext.Current.Pending();
        }
    }
}
