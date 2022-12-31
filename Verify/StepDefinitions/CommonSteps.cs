using Newtonsoft.Json.Linq;
using NUnit.Framework;
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

            String id = Guid.NewGuid().ToString();

            this._awsContext.payload["messageId"] = id;
            this._awsContext.messsageID = id;
            this.LogAndReport("Payload messageID: " + id);
        }


        [Given(@"I open the ""([^""]*)"" json")]
        public void GivenIOpenTheJson(string fileName)
        {
            this._awsContext.payload = this.readJSONfile(fileName + ".json");
        }

        [Given(@"I open the ""([^""]*)"" json in folder ""([^""]*)""")]
        public void GivenIOpenTheJsonInFolder(string fileName, string folderName)
        {
            this._awsContext.payload = this.readJSONfile(folderName  +@"/" + fileName + ".json");
            this.LogAndReport(this._awsContext.payload.ToString());
        }



        [When(@"I publish the json to the ""([^""]*)"" arn")]
        public async Task WhenIPublishTheJsonToTheArnAsync(string arnName)
        {
            await this._awsContext.SNSClient.pubTopicAsyncWithAttr(this._awsContext.payload.ToString(),
                arnName,
                this._awsContext.snsMessageAttributes);
        }

        [Then(@"I look for the JSON response in ""([^""]*)""")]
        public async Task ThenILookForTheJSONResponseIn(string sqsUrl)
        {
            this._awsContext.SQSClient.sqsURL = sqsUrl;

            int attempts = 30;
            int waitBetweenAttempts = 5000;
            String log = null;
            for (int i = 0; i < attempts; i++)
            {
                Console.WriteLine("Starting log search attempt " + i + "/" + attempts);
                Thread.Sleep(waitBetweenAttempts);
                log = await this._awsContext.SQSClient.FindLogWithMessageId(this._awsContext.messsageID);
                if (log != null)
                {
                    Console.WriteLine("log Found!");
                    Console.WriteLine(log);
                    if (AWSContext.troubleShootReports) this.LogAndReport(log);
                    this._awsContext.response = JObject.Parse(log);
                    break;
                }
            }

            Assert.NotNull(log);

        }



        [Then(@"I look for the messageId in CloudWatchLogs group ""([^""]*)""")]
        public async Task ThenILookForTheMessageIdInCloudWatchLogsGroupAsync(string groupName)
        {
            int attempts = 20;
            int waitBetweenAttempts = 5000;
            String log = null;
            for (int i = 0; i < attempts; i++)
            {
                Console.WriteLine("Starting log search attempt " + i + "/" + attempts);
                Thread.Sleep(waitBetweenAttempts);
                log = await this._awsContext.CloudWatchLogsClient.getLogByMessageId(this._awsContext.messsageID, groupName);
                if (log != null)
                {
                    Console.WriteLine("log Found!");
                    Console.WriteLine(log);
                    if (AWSContext.troubleShootReports) this.LogAndReport(log);
                    this._awsContext.response = JObject.Parse(log);
                    break;
                }
            }

            if(AWSContext.troubleShootReports){
                if (this._awsContext.CloudWatchLogsClient.errorLog.Count() > 0)
                    this.LogAndReport("Error / Exception found in CW logs");
                foreach (String cwLog in this._awsContext.CloudWatchLogsClient.errorLog)
                {
                    this.LogAndReport(cwLog);
                }
            }
 
            Assert.NotNull(log);

        }

        [Then(@"I verify the key ""([^""]*)"" with the value ""([^""]*)""")]
        public void ThenIVerifyTheKeyWithTheValue(string key, string value)
        {
            if (value.ToLower().Replace(" ", "").Equals("na")) value = "";

            String real = value.ToLower().Replace(" ", "");
            String expected = this._awsContext.response.SelectToken(key).ToString().ToLower().Replace(" ","");
            this.LogAndReport("Expected value: " + value);
            this.LogAndReport("Real value: " + this._awsContext.response.SelectToken(key));

            Assert.AreEqual(real, expected);
        }

        [When(@"I test the lambda function ""([^""]*)""")]
        public async Task WhenITestTheLambdaFunction(string funcName)
        {
            String resp = await this._awsContext.LambdaClient.InvokeFunctionAsync(funcName, this._awsContext.payload.ToString());

            this.LogAndReport("Lambda response: " + resp);
            this._awsContext.response = JObject.Parse(resp);
        }

        [Then(@"I verify the lambda response ""([^""]*)""")]
        public void ThenIVerifyTheLambdaResponse(string numberOfFailures)
        {
            JToken[] failures = this._awsContext.response.SelectToken("batchItemFailures").ToArray();

            Assert.AreEqual(int.Parse(numberOfFailures), failures.Length);
        }

    }
}
