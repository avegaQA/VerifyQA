
using Verify.AWSHandlers;

namespace Verify.Context
{
    public class AWSContext
    {
        public EventBridgeHandler EventBridgeClient { get; set; }

        public SNShandler SNSClient { get; set; }

        public CloudWatchLogsHandler CloudWatchLogsClient { get; set; }

    }
}
