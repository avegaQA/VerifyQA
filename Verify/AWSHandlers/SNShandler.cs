
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verify.Hooks;

namespace Verify.AWSHandlers
{
    public class SNShandler : HandlerBase
    {
        public AmazonSimpleNotificationServiceClient client;

        public SNShandler()
        {
            var AWS_ACCESS_KEY_ID       = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var AWS_SECRET_ACCESS_KEY   = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            var credentials = new BasicAWSCredentials(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY);

            this.client = new AmazonSimpleNotificationServiceClient(credentials, this.region);
        }

        public async Task<Boolean> CheckIfTopicExists(String topicName)
        {
            string nextToken = string.Empty;

            do
            {
                var response = await client.ListTopicsAsync(nextToken);
                foreach (var topic in response.Topics)
                {
                    Console.WriteLine($"{topic.TopicArn}");
                    if (topic.TopicArn.Contains(topicName))
                    {
                        return true;
                    }
                }
                nextToken = response.NextToken;
            }
            while (!string.IsNullOrEmpty(nextToken));
            return false;
        }

        public async Task GetTopicListAsync()
        {
            string nextToken = string.Empty;

            do
            {
                var response = await client.ListTopicsAsync(nextToken);
                DisplayTopicsList(response.Topics);
                nextToken = response.NextToken;
            }
            while (!string.IsNullOrEmpty(nextToken));
        }

        public void DisplayTopicsList(List<Topic> topicList)
        {
            foreach (var topic in topicList)
            {
                //Console.WriteLine($"{topic.TopicArn}");
                this.LogAndReport("The Topic ARN is " + topic.TopicArn);
            }
        }

        public void closeClient()
        {
            this.client.Dispose();
        }
    }
}

