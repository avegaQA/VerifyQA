using Newtonsoft.Json.Linq;
using System;
using TechTalk.SpecFlow;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    public class Retrieve_LicensePSVDataStepDefinitions : TestBase
    {
        private AWSContext _awsContext;

        public Retrieve_LicensePSVDataStepDefinitions(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [When(@"I publish the payload to the ""([^""]*)"" arn")]
        public async Task WhenIPublishThePayloadToTheArnAsync(string arn)
        {
            Random rd = new Random();

            String id = rd.Next(10000, 90000) + "_AUTOTEST";

            JObject payload = this.readJSONfile("Retrieve_LicensePSVData.json");

            payload["messageId"]     = id;
            payload["correlationId"] = id;
            payload["causationId"]   = id;

            this.LogAndReport("Payload message ID: " + payload["messageId"]);

            await this._awsContext.SNSClient.pubTopicAsync(payload.ToString(), arn);
        }

        [Then(@"I verify the Cloudwatch logs")]
        public void ThenIVerifyTheCloudwatchLogs()
        {
            
        }
    }
}
