using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
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
    }
}
