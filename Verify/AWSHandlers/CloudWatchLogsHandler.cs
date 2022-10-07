using Amazon.CloudWatchLogs;
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



        public void closeClient()
        {
            this.client.Dispose();
        }
    }
}
