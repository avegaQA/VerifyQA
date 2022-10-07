using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    public class SNSStepDefinitions : TestBase
    {
        private AWSContext _awsContext;

        public SNSStepDefinitions(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [When(@"I list all topics")]
        public async Task WhenIListAllTopics()
        {
            await this._awsContext.SNSClient.GetTopicListAsync();
            
        }

        [Then(@"I check ""([^""]*)"" is available")]
        public async Task ThenICheckIsAvailable(string topicName)
        {
            Boolean exist = await this._awsContext.SNSClient.CheckIfTopicExists(topicName);

            Console.WriteLine("Found? " + exist);
            Assert.True(exist);
        }
    }
}
