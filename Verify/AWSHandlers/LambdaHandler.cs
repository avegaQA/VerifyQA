using Amazon.Lambda;
using Amazon.Lambda.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verify.AWSHandlers
{
    public class LambdaHandler : HandlerBase
    {
        public AmazonLambdaClient client;

        public String messageID = "";

        public LambdaHandler()
        {
            var credentials = this.GetCredentials();

            this.client = new AmazonLambdaClient(credentials, this.region);
        }

        public async Task<string> InvokeFunctionAsync( string functionName, string parameters)
        {
            var payload = parameters;
            var request = new InvokeRequest
            {
                FunctionName = functionName,
                Payload = payload,
            };

            var response = await client.InvokeAsync(request);
            MemoryStream stream = response.Payload;
            string returnValue = Encoding.UTF8.GetString(stream.ToArray());
            return returnValue;
        }

        public void closeClient()
        {
            this.client.Dispose();
        }
    }
}
