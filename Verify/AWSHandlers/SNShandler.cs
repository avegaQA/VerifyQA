
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verify.Context;
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
                this.LogAndReport("The Topic ARN is " + topic.TopicArn);
            }
        }

        public async Task pubTopicAsync(String message, String topicArn)
        {
            Dictionary<string, MessageAttributeValue> messageAttributes = new Dictionary<string, MessageAttributeValue>();

            messageAttributes.Add("message_destinations", new MessageAttributeValue() { 
                DataType = "String.Array",
                StringValue = "[\"null-primary-source\"]"
            });

            PublishRequest request = new PublishRequest(topicArn, message, "This is an automated test");

            request.MessageAttributes = messageAttributes;
            PublishResponse response =  await client.PublishAsync(request);
            this.LogAndReport("Message ID: " + response.MessageId);
            this.LogAndReport("Status: " + response.HttpStatusCode.ToString());
        }

        public async Task pubTopicAsyncWithAttr(String message, String topicArn, Dictionary<string, MessageAttributeValue> messageAttributes)
        {

            PublishRequest request = new PublishRequest(topicArn, message, "This is an automated test");

            request.MessageAttributes = messageAttributes;
            PublishResponse response = await client.PublishAsync(request);
            this.LogAndReport("Message ID: " + response.MessageId);
            this.LogAndReport("Status: " + response.HttpStatusCode.ToString());
            if (AWSContext.troubleShootReports) this.LogAndReport("Message: " + message);
        }

        public void closeClient()
        {
            this.client.Dispose();
        }
    }
}

