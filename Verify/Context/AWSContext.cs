
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json.Linq;
using Verify.AWSHandlers;

namespace Verify.Context
{
    public class AWSContext
    {
        public static Boolean reportLog = true;

        public static Boolean consoleLog = true;

        public static Boolean troubleShootReports = false;

        public String messsageID { get; set; }
        public EventBridgeHandler EventBridgeClient { get; set; }

        public SNShandler SNSClient { get; set; }

        public CloudWatchLogsHandler CloudWatchLogsClient { get; set; }

        public RDSHandler RDSClient { get; set; }

        public JObject payload { get; set; }

        public JObject response { get; set; }

        //private readonly ISecretsManager _secretsManager;

        public Dictionary<string, MessageAttributeValue> snsMessageAttributes = new Dictionary<string, MessageAttributeValue>();

    }
}
