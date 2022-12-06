# User Story: https://dev.azure.com/symplr/Provider%20Management/_sprints/taskboard/Gondor/Provider%20Management/2022%20PI%204/Sprint%201?workitem=459745

@SNS
@SQS
@SRS_459745.009
Feature: IOWA Board of Medicine Unhappy paths

@SRS_459745.015
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

@SRS_459745.001
@SRS_502180.011
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
	And I verify the message destinations field
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key         | value                      |
		| messageType | PsvDaqMessage_Misdelivered |

#***************************************************************************************************************************************

@SRS_502180.012
Scenario: RequestForUnsupportedLicense_Received

	Given I open the "IOWABoardOfMedicine" json
	And I prepare the JSON data
		| key                                    | value |
		| data.searchAttributes.jurisdiction     | TA    |
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

@SRS_459745.018 
@SRS_459745.020 @SRS_459745.019
Scenario Outline: PrimarySourceDataAcquisition_Failed

	Given I open the "<JSONname>" json
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key          | value                               |
		| messageType  | PrimarySourceDataAcquisition_Failed |

Examples:
	| JSONname                      |
	| LicenseNumberCorrupted        |
	| IOWABoardOfMedicineIncomplete |

#***************************************************************************************************************************************

Scenario Outline: PrimarySourceDataAcquisition_Failed check message

	Given I open the "<JSONname>" json
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

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

#		Requirements missing to automate:
#		Difficult---------------------------- Requires corrupted image for testing TBD
#		ConnectingToPrimarySourceSystem_Failed  SRS_459745.013  SRS_459745.014
#		StorageOfProofArtifact_Failed			SRS_459745.107	https://symplr.visualstudio.com/6d691373-87da-4a72-ae51-3f0d823947c8/_workitems/edit/507281
#		ParsingPrimarySourceData_Failed			SRS_459745.016  SRS_459745.017