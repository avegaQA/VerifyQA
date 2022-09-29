using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using Verify.AWSHandlers;

namespace Verify.StepDefinitions
{
    [Binding]
    public class EventBridgeStepDefinitions
    {
        public EventBridgeHandler eventBridgeHandler;

        [Given(@"I log in into AWS EventBridge")]
        public void GivenILogInIntoAWSEventBridge()
        {
            eventBridgeHandler = new EventBridgeHandler();
        }

        [When(@"I list all rules")]
        public async Task WhenIListAllRules()
        {
            await eventBridgeHandler.ListAllRules();
        }

        [Then(@"I check ""([^""]*)"" is created")]
        public async Task ThenICheckIsCreated(string name)
        {
            var found = await eventBridgeHandler.CheckItRuleExist(name);

            Console.WriteLine("Rule was found? " + found);

            Assert.True(found); 
        }

        [Then(@"I close the AWS EventBridge")]
        public void ThenICloseTheAWSEventBridge()
        {
            eventBridgeHandler.closeClient();
        }
    }
}
