using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using Verify.AWSHandlers;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    public class EventBridgeStepDefinitions
    {

        private AWSContext _awsContext;

        public EventBridgeStepDefinitions(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [When(@"I list all rules")]
        public async Task WhenIListAllRules()
        {
            await this._awsContext.EventBridgeClient.ListAllRules();
        }

        [Then(@"I check ""([^""]*)"" is created")]
        public async Task ThenICheckIsCreated(string name)
        {
            var found = await this._awsContext.EventBridgeClient.CheckItRuleExist(name);

            Console.WriteLine("Rule was found? " + found);

            Assert.True(found); 
        }

    }
}
