using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    public class CommonSteps : TestBase
    {

        private AWSContext _awsContext;

        public CommonSteps(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [Given(@"I load the messageId")]
        public void GivenILoadTheMessageId()
        {
            Random rd = new Random();

            String id = rd.Next(10000, 90000) + "_AUTOTEST";

            this._awsContext.payload["messageId"] = id;
            this._awsContext.messsageID = id;
            this.LogAndReport("Payload messageID: " + id);
        }


        [Given(@"I open the ""([^""]*)"" json")]
        public void GivenIOpenTheJson(string fileName)
        {
            this._awsContext.payload = this.readJSONfile(fileName + ".json");
        }


        [When(@"I publish the json to the ""([^""]*)"" arn")]
        public async Task WhenIPublishTheJsonToTheArnAsync(string arnName)
        {
            await this._awsContext.SNSClient.pubTopicAsyncWithAttr(this._awsContext.payload.ToString(), 
                arnName , 
                this._awsContext.snsMessageAttributes);
        }


        [Then(@"I look for the messageId in CloudWatchLogs group ""([^""]*)""")]
        public async Task ThenILookForTheMessageIdInCloudWatchLogsGroupAsync(string groupName)
        {
            int attempts = 10;
            String log;
            for(int i = 0; i < attempts; i++)
            {
                Thread.Sleep(10000);
                log = await this._awsContext.CloudWatchLogsClient.getLogByMessageId(this._awsContext.messsageID, groupName);
                if(log != null)
                {
                    if(AWSContext.troubleShootReports) this.LogAndReport(log);
                    this._awsContext.response = JObject.Parse(log);
                    break;
                }
            }
            
        }


    }
}
