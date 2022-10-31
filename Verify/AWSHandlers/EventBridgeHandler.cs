using Amazon;
using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verify.Hooks;

namespace Verify.AWSHandlers
{
    public class EventBridgeHandler : HandlerBase
    {
        private AmazonEventBridgeClient client;
        public EventBridgeHandler()
        {
            Console.WriteLine("Creating EventBridge client...");

            var credentials = this.GetCredentials();

            this.client = new AmazonEventBridgeClient(credentials, this.region);

            Console.WriteLine("EventBridge client created!");
        }

        public async Task ListAllRules()
        {
            Console.WriteLine("Getting rules....");

            try
            {
                var request = new ListRulesRequest()
                {
                    EventBusName = "default",
                    Limit = 10
                };

                var response = await this.client.ListRulesAsync(request);

                Console.WriteLine(response.ToString());

                var rules = response.Rules;

                foreach (var rule in rules)
                {
                    this.LogAndReport("The rule name is " + rule.Name);
                    this.LogAndReport("The rule ARN is " + rule.Arn);
                };
                Console.WriteLine("Rules Printed....");
            }
            catch(AmazonEventBridgeException e)
            {
                Console.WriteLine(e.ToString());
            };

        }

        public async Task<Boolean> CheckItRuleExist(String ruleName)
        {
            Console.WriteLine("Getting rules....");

            try
            {
                var request = new ListRulesRequest()
                {
                    EventBusName = "default",
                    Limit = 10
                };

                var response = await this.client.ListRulesAsync(request);

                Console.WriteLine(response.ToString());

                var rules = response.Rules;

                foreach (var rule in rules)
                {
                    Console.WriteLine("The rule name is " + rule.Name);
                    Console.WriteLine("The rule ARN is " + rule.Arn);

                    if (rule.Name.Contains(ruleName))
                    {
                        return true;
                    }
                };
                Console.WriteLine("Rules Printed....");
                return false;
            }
            catch (AmazonEventBridgeException e)
            {
                Console.WriteLine(e.ToString());
            };

            return false;

        }

        public void closeClient()
        {
            this.client.Dispose();
        }

    }

}
