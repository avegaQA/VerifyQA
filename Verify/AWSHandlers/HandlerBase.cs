using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Verify.Context;
using Verify.Hooks;

namespace Verify.AWSHandlers
{
    public class HandlerBase
    {
        public RegionEndpoint region = RegionEndpoint.USEast2;

        public void LogAndReport(String log)
        {
            
            if (AWSContext.consoleLog) Console.WriteLine(log);
            if (AWSContext.reportLog) ReportHooks.defineTestText(log);
        }

        public BasicAWSCredentials GetCredentials()
        {
            var AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            return new BasicAWSCredentials(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY);
        }
    }
}
