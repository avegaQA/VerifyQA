using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verify.AWSHandlers
{
    public class SQSHandler : HandlerBase
    {
        public AmazonSQSClient client;

        public String sqsURL { get; set; }

        public SQSHandler()
        {
            var credentials = this.GetCredentials();

            this.client = new AmazonSQSClient(credentials, this.region);
        }

        public async Task<ReceiveMessageResponse> GetMessage(int maxNumberOfMessages)
        {
            return await this.client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = this.sqsURL,
                MaxNumberOfMessages = maxNumberOfMessages,
                WaitTimeSeconds = 0
            });
        }

        public async Task<String> FindLogWithMessageId(String messageId)
        {
            ReceiveMessageResponse messages =  await this.client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = this.sqsURL,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 0
            });

            foreach (Message message in messages.Messages)
            {
                if (message.Body.ToString().Contains(messageId))
                {
                    Console.WriteLine("Message found!");
                    this.LogAndReport("Found in: " + message.MessageId);
                    return message.Body.ToString();
                }
            }

            return null;
        }

        public void closeClient()
        {
            this.client.Dispose();
        }

    }
}
