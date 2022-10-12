﻿
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json.Linq;
using Verify.AWSHandlers;

namespace Verify.Context
{
    public class AWSContext
    {
        public static Boolean troubleShootReports = true;

        public String messsageID { get; set; }
        public EventBridgeHandler EventBridgeClient { get; set; }

        public SNShandler SNSClient { get; set; }

        public CloudWatchLogsHandler CloudWatchLogsClient { get; set; }

        public JObject payload { get; set; }

        public JObject response { get; set; }

        public Dictionary<string, MessageAttributeValue> snsMessageAttributes = new Dictionary<string, MessageAttributeValue>();

    }
}
