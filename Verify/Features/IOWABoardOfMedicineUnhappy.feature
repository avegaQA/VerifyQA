# User Story: https://dev.azure.com/symplr/Provider%20Management/_sprints/taskboard/Gondor/Provider%20Management/2022%20PI%204/Sprint%201?workitem=459745

@SNS
@SQS
Feature: IOWA Board of Medicine Unhappy paths

Scenario: ProviderLicenseMatch_NotFound

	Given I open the "IOWABoardOfMedicine" json
	And I prepare the JSON data
		| key                                                | value      |
		| data.searchAttributes.individualNames[0].firstName | sefsefsese |
		| data.searchAttributes.individualNames[0].lastName  | sefssef    |
		| data.searchAttributes.licenseNumber                | sefesfdr   |
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key         | value                         |
		| messageType | ProviderLicenseMatch_NotFound |

#***************************************************************************************************************************************

Scenario: PsvDaqMessage_Misdelivered

	Given I open the "IOWABoardOfMedicine" json
	And I prepare the JSON data
		| key                                                | value                                                |
		| data.searchAttributes.individualNames[0].firstName | Alfonso                                              |
		| data.searchAttributes.individualNames[0].lastName  | Vega                                                 |
		| data.searchAttributes.licenseNumber                | QA                                                   |
		| destination                                        | PSV-DAQ \| DAQ Workers \| MiddleEarthBoardOfMedicine |
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key         | value                      |
		| messageType | PsvDaqMessage_Misdelivered |

#***************************************************************************************************************************************

Scenario: RequestForUnsupportedLicense_Received

	Given I open the "IOWABoardOfMedicine" json
	And I prepare the JSON data
		| key                                    | value |
		| data.searchAttributes.primarySource    | TA    |
		| data.searchAttributes.fieldOfLicensure | 000   |
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key         | value                                 |
		| messageType | RequestForUnsupportedLicense_Received |

#***************************************************************************************************************************************

Scenario: PrimarySourceDataAcquisition_Failed

	Given I open the "IOWABoardOfMedicineIncomplete" json
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key         | value                               |
		| messageType | PrimarySourceDataAcquisition_Failed |

#***************************************************************************************************************************************