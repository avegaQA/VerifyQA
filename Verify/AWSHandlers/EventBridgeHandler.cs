using Amazon;
using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verify.AWSHandlers
{
    public class EventBridgeHandler
    {
        public AmazonEventBridgeClient client;
        public EventBridgeHandler()
        {
            Console.WriteLine("Creating EventBridge client...");

            var AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            var credentials = new BasicAWSCredentials(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY);

            this.client = new AmazonEventBridgeClient(credentials, RegionEndpoint.USEast2);

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
                    Console.WriteLine("The rule name is " + rule.Name);
                    Console.WriteLine("The rule ARN is " + rule.Arn);
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
