#US: https://dev.azure.com/symplr/Provider%20Management/_backlogs/backlog/Gondor/Features/?workitem=576412

Feature: Invalid Schema validation
@SNS
@SQS
@576412.007
Scenario Outline: Invalid Schema validating error message (PrimarySourceDataAcquisition_Failed)

	Given I open the "<JSONname>" json in folder "IOWAboardOfMedicine"
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-gondordev01-vfy-psvDaqRequests-topic" arn

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key          | value                               |
		| messageType  | PrimarySourceDataAcquisition_Failed |
		| data.message | <message>                           |

Examples:
	| JSONname                 | message                                                                                                                                    |
	| LicenseNumberCorrupted   | Unable to proceed with PSV data retrieval due to incomplete provider license information. The property licenseNumber is missing.           |
	| EmptyIndividualNames     | Unable to proceed with PSV data retrieval due to incomplete provider license information. A value in individualNames property is required. |
	| WrongType                | Unable to proceed with PSV data retrieval due to incomplete provider license information. individualNames property has a wrong type.       |
	| SearchAttributeCorrupted | Unable to proceed with PSV data retrieval due to incomplete provider license information. The property searchAttributes is missing.        |

#***************************************************************************************************************************************
