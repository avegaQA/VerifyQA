# Introduction 
This document explains the test automation framework for testing AWS projects. This framework is used when we want to test directly AWS code without using any API nor UI. This framework uses the AWS SDK for interfacing directly to thhe AWS services.

# Getting Started
1.	Installations 
    - Visual Studio 2019 or higher
2.	Visual Studio Extensions
    - Specflow for Visual Studio

# Build and Test
- Clone this repository and open Verify.sln file using Visual Studio
- Build solution using Visual Studio Build -> Build Solution menu
- Go to test explorer, select the test and run.
- Check report on the Report Folder

# Create new test
**Define tests/scenarios** -> **Stage test data** -> **Create the required AWS methods** -> **Implement steps** -> **Run tests**

## TestProject structure
The test solution has the following structure.
- AWSHandlers
- Context
- Features
- Hooks
- Reports
- StepDefinitions
- TestData
## AWSHandlers
We define one class for each AWS Service using the AWS SDK dependencies for each service. All AWS relates logic should be contain in this classes.

## Authentication
You need to define the **AWS_ACCESS_KEY_ID** and the **AWS_SECRET_ACCESS_KEY** as enviroment variables.

## Defining test scenarios
In this phase, an automation engineer should create/use a feature file and write scenarios as required. With the test project, all features files and scenarios should be put under **Features** folder.
Each scenario can be provided with tags as required. In this project each AWS service will have a tag wich is being used by the AWSHooks class to start and dispose each client object. As an example in the following Scenario the test requires to connect to the SNS and to CLoudWatch. 

**An example scenario**
```
@SNS
@CloudWatchLogs
Scenario: Search in a non existant board
	Given I open the "IOWABoardOfMedicine" json
	And I load the first name "Alfonso"
	And I load the last name "Vega"
	And I load the license number "QA"
	And I load the destination to "PSV-DAQ | DAQ Workers | MiddleEarthBoardOfMedicine"
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the messageId in CloudWatchLogs group "/aws/lambda/pdm-dev-vfy-daqWorkers-IABoardOfMedicine-func"
	And I parse the json response for IOWA Board of medicine
	And I verify the key "destination" with the value "PSV-DAQ | Failures"
	And I verify the key "messageType" with the value "PsvDaqMessage_Misdelivered"
```

## Stage test data
In this phase, if the test requires data or a JSON template save this under the TestData Folder

## Create the required AWS methods
- In this phase, all AWS related classes should be saved under the AWSHandler following the naming convention of <AWSName>Handler. (For example: SNSHandler, EventBridgeHandler)
- Also each page class should inherit the base page class called **HandlerBase.cs**
- The constructor should create the client on the constructor of the clasee. See an example below.
    
    ```
        public SNShandler()
        {
            var credentials = this.GetCredentials();

            this.client = new AmazonSimpleNotificationServiceClient(credentials, this.region);
        }
    ```
- AWS handler class should also have a closeClient method as shown in the following:
    
    ```
        public void closeClient()
        {
            this.client.Dispose();
        }
    ```
- In the AWSHooks class add the required methods to start and close the respective client.
    ```
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
    ```

## Implement steps
This is a significant phase in which step definitions for the scenarios are done. Please see the below given code snippet which shows how page methods are accessed from step definitions.

    ```
        [When(@"I list all rules")]
        public async Task WhenIListAllRules()
        {
            await this._awsContext.EventBridgeClient.ListAllRules();
        }
    ```
In the above code snippet, there is an example for calling an EventBridge method

## Run test
Build solution, go to test explorer, select the test and run.
