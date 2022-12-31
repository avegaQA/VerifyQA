﻿using TechTalk.SpecFlow;
using Verify.AWSHandlers;
using Verify.Context;

namespace Verify.Hooks
{
    [Binding]
    public sealed class AWSHooks
    {
        private AWSContext _awsContext;
        public AWSHooks(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [BeforeScenario("EventBridge")]
        public void BeforeScenarioEventBridge()
        {
            this._awsContext.EventBridgeClient = new EventBridgeHandler();
        }

        [AfterScenario("EventBridge")]
        public void AfterScenarioEventBridge()
        {
            this._awsContext.EventBridgeClient.closeClient();
        }

        [BeforeScenario("SNS")]
        public void BeforeScenarioSNS()
        {
            this._awsContext.SNSClient = new SNShandler();
        }

        [AfterScenario("SNS")]
        public void AfterScenarioSNS()
        {
            this._awsContext.SNSClient.closeClient();
        }

        [BeforeScenario("SQS")]
        public void BeforeScenarioSQS()
        {
            this._awsContext.SQSClient = new SQSHandler();
        }

        [AfterScenario("SQS")]
        public void AfterScenarioSQS()
        {
            this._awsContext.SQSClient.closeClient();
        }

        [BeforeScenario("CloudWatchLogs")]
        public void BeforeScenarioCloudWatchLogs()
        {
            this._awsContext.CloudWatchLogsClient = new CloudWatchLogsHandler();
        }

        [AfterScenario("CloudWatchLogs")]
        public void AfterScenarioCloudWatchLogs()
        {
            this._awsContext.CloudWatchLogsClient.closeClient();
        }

        [BeforeScenario("Lambda")]
        public void BeforeScenarioLambda()
        {
            this._awsContext.LambdaClient = new LambdaHandler();
        }

        [AfterScenario("Lambda")]
        public void AfterScenarioLambda()
        {
            this._awsContext.LambdaClient.closeClient();
        }

        [BeforeScenario("AWSCLI")]
        public void BeforeScenarioAWSCLI()
        {
            this._awsContext.awscliHandler = new AWSCLIHandler();
        }

    }
}