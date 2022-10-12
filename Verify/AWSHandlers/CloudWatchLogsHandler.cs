using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verify.AWSHandlers
{
    public class CloudWatchLogsHandler : HandlerBase
    {
        public AmazonCloudWatchLogsClient client;

        public CloudWatchLogsHandler()
        {
            var AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            var credentials = new BasicAWSCredentials(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY);

            this.client = new AmazonCloudWatchLogsClient(credentials, this.region);
        }

        public async Task<List<LogStream>> getLastStreams(String groupName, int amount)
        {
            DescribeLogStreamsResponse result;
            String nextToken = null;

            result = await this.client.DescribeLogStreamsAsync(new DescribeLogStreamsRequest
            {
                LogGroupName = groupName,
                Descending = true,
                OrderBy = "LastEventTime",
                Limit = amount,
                NextToken = nextToken
            });

            return result.LogStreams;

        }

        public async Task<List<string>> getCWlogEventsAsync(String logGroupName, String logStreamName)
        {
            List<string> eventMessages = new List<string> { };

            GetLogEventsRequest eventRequest = new GetLogEventsRequest()
            {
                LogGroupName = logGroupName,
                LogStreamName = logStreamName,
                StartFromHead = true
            };

            GetLogEventsResponse response = await client.GetLogEventsAsync(eventRequest);

            List<OutputLogEvent> logEvents = response.Events;

            foreach(OutputLogEvent logEvent in logEvents)
            {
                eventMessages.Add(logEvent.Message);
            }

            return eventMessages;

        }

        public async Task<string> getLogByMessageId(String messageid, String groupName)
        {
            List<string> logMessages = new List<string> { };

            List<LogStream> logStreams = await getLastStreams(groupName, 3);

            foreach(LogStream logStream in logStreams)
            {
                logMessages = await getCWlogEventsAsync(groupName, logStream.LogStreamName);

                foreach(String logMessage in logMessages)
                {
                    if (logMessage.Contains(messageid) && logMessage.Contains("PSV result parsed")) return logMessage;
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
