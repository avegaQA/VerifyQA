using TechTalk.SpecFlow;
using Verify.AWSHandlers;
using Verify.Context;

namespace Verify.Hooks
{
    [Binding]
    public sealed class AWSHooks
    {
        private AWSContext _awsContext;
        public AWSHooks(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [BeforeScenario("EventBridge")]
        public void BeforeScenarioEventBridge()
        {
            this._awsContext.EventBridgeClient = new EventBridgeHandler();
        }

        [AfterScenario("EventBridge")]
        public void AfterScenarioEventBridge()
        {
            this._awsContext.EventBridgeClient.closeClient();
        }


    }
}