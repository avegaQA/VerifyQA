# User Story: https://dev.azure.com/symplr/Provider%20Management/_sprints/taskboard/Gondor/Provider%20Management/2022%20PI%204/Sprint%201?workitem=459745

@SNS
@SQS
#@CloudWatchLogs
Feature: IOWA Board of Medicine

Scenario Outline: Search doctor

	Given I open the "IOWABoardOfMedicine" json
	And I prepare the JSON data
		| key                                                | value           |
		| data.searchAttributes.individualNames[0].firstName | <firstName>     |
		| data.searchAttributes.individualNames[0].lastName  | <lastName>      |
		| data.searchAttributes.licenseNumber                | <licenseNumber> |
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	#Then I look for the messageId in CloudWatchLogs group "/aws/lambda/pdm-dev-vfy-daqWorkers-IABoardOfMedicine-func"
	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I check for error messages
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key                                | value            |
		| data.licenseDetails.issueDate      | <issueDate>      |
		| data.licenseDetails.expirationDate | <expirationDate> |
		| data.licenseDetails.licenseNumber  | <licenseNumber>  |

	Then I get the access token
	And I verify the proof of artifact
	#And I check the disciplinary records to match with "<disciplinaryActionRecords>"

Examples:
	| firstName | lastName  | licenseNumber | issueDate           | expirationDate | disciplinaryActionRecords |
	| Fred      | Goldblatt | DO-02020      | Jan 31 1984 12:00AM | 11/01/2024     | 0                         |
	| Sally     | Smith     | R-12632       | 06/08/2022          | 06/30/2024     | 0                         |
	| Kathleen  | Jones     | MD-32008      | Aug 12 1997 12:00AM | 12/01/2024     | 0                         |
	| Abigail   | Scrogum   | R-12485       | 05/20/2022          | 06/30/2025     | 0                         |
	| Kirk      | Smith     | NA            | NA                  | 04/26/2002     | 2                         |

#***************************************************************************************************************************************

Scenario: Cross-product of names

	Given I open the "IOWABoardOfMedicine" json
	And I prepare the JSON data
		| key                                                | value     |
		| data.searchAttributes.individualNames[0].firstName | Fred      |
		| data.searchAttributes.individualNames[1].lastName  | Goldblatt |
		| data.searchAttributes.licenseNumber                | DO-02020  |
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I check for error messages
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key                                | value               |
		| data.licenseDetails.issueDate      | Jan 31 1984 12:00AM |
		| data.licenseDetails.expirationDate | 11/01/2024          |
		| data.licenseDetails.licenseNumber  | DO-02020            |

	Then I get the access token
	And I verify the proof of artifact

#***************************************************************************************************************************************




