# User Story: https://dev.azure.com/symplr/Provider%20Management/_sprints/taskboard/Gondor/Provider%20Management/2022%20PI%204/Sprint%201?workitem=459745
#dotnet test --filter Category=RunThis

Feature: IOWA Board of Medicine

@SNS
@SQS
@SRS_459745.003 @SRS_459745.004
@SRS_459745.005 @SRS_459745.006
Scenario Outline: Search by first name, last name and License number

	Given I open the "IOWABoardOfMedicine" json in folder "IOWAboardOfMedicine"
	And I prepare the JSON data
		| key                                                | value           |
		| data.searchAttributes.individualNames[0].firstName | <firstName>     |
		| data.searchAttributes.individualNames[0].lastName  | <lastName>      |
		| data.searchAttributes.licenseNumber                | <licenseNumber> |
		| data.licenseHash                                   | <licenseHash>   |
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-gondordev01-vfy-psvDaqRequests-topic" arn
	#arn:aws:sns:us-east-2:379493731719:pdm-gondordev01-vfy-psvDaqRequests-topic
	#arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I check for error messages
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key                                | value                   |
		| data.licenseDetails.issueDate      | <issueDate>             |
		| data.licenseDetails.expirationDate | <expirationDate>        |
		| data.licenseDetails.licenseNumber  | <licenseNumber>         |
		| messageType                        | LicensePSVData_Acquired |

	Then I get the access token
	And I verify the proof of artifact
	And I verify the Raw HTML
	And I check the disciplinary records to match with "<disciplinaryActionRecords>"

Examples:
	| firstName | lastName  | licenseNumber | issueDate           | expirationDate | disciplinaryActionRecords | licenseHash                      |
	| Fred      | Goldblatt | DO-02020      | Jan 31 1984 12:00AM | 11/01/2024     | 0                         | x02                              |
	| Sally     | Smith     | R-12632       | 06/08/2022          | 06/30/2024     | 0                         | F74BF2B5C91BB04243432CD8AF30A7D2 |
	| Kathleen  | Jones     | MD-32008      | Aug 12 1997 12:00AM | 12/01/2024     | 0                         | x01                              |
	| Abigail   | Scrogum   | R-12485       | 05/20/2022          | 06/30/2025     | 0                         | x02                              |
	| Kirk      | Smith     | NA            | NA                  | 04/26/2002     | 2                         | F74BF2B5C91BB04243432CD8AF30A7D2 |


#***************************************************************************************************************************************

@SNS
@SQS
@SRS_459745.001
Scenario Outline: jurisdiction, LicenseType and FieldOFLicensure valid combinations

	Given I open the "IOWABoardOfMedicine" json in folder "IOWAboardOfMedicine"
	And I prepare the JSON data
		| key                                                | value              |
		| data.searchAttributes.individualNames[0].firstName | Sally              |
		| data.searchAttributes.individualNames[0].lastName  | Smith              |
		| data.searchAttributes.licenseNumber                | R-12632            |
		| data.searchAttributes.jurisdiction                 | <jurisdiction>     |
		| data.searchAttributes.fieldOfLicensure             | <FieldOfLicensure> |
		| data.searchAttributes.licenseType                  | <LicenseType>      |
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-gondordev01-vfy-psvDaqRequests-topic" arn

	Then I look for the JSON response in "https://sqs.us-east-2.amazonaws.com/379493731719/pdm-dev-vfy-testAutomationSubscriber-queue"
	And I check for error messages
	And I parse the json response for IOWA Board of medicine
	And I verify the JSON response
		| key                           | value                   |
		| messageType                   | LicensePSVData_Acquired |
		| data.licenseDetails.issueDate | 06/08/2022              |


	Then I get the access token
	And I verify the proof of artifact
	And I verify the Raw HTML

Examples:
	| jurisdiction | FieldOfLicensure | LicenseType  |
	| IA           | 010              | state_issued |
	| IA           | 015              | state_issued |
	| IA           | 020              | state_issued |
	| IA           | 025              | state_issued |
	
	#state_issued or state?

#***************************************************************************************************************************************

@Lambda
@SRS_459745.008
Scenario Outline: Check for batchItemFailures
	Given I open the "<JSONname>" json in folder "IOWAboardOfMedicine"

	When I test the lambda function "pdm-dev-vfy-daqWorkers-IABoardOfMedicine-func"

	Then I verify the lambda response "<NumberOfFailures>"


Examples:
	| JSONname                | NumberOfFailures |
	| IOWAHappyLambdaTester   | 0                |
	| IOWAUnhappyLambdaTester | 1                |

#***************************************************************************************************************************************

#Requirements missing to automate:
#SRS_459745.007 - Database check

#***************************************************************************************************************************************

#Deprecated
#SRS_459745.002



