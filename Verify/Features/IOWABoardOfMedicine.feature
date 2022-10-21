@SNS
@CloudWatchLogs
Feature: IOWA Board of Medicine

Scenario Outline: Search doctor
	Given I open the "IOWABoardOfMedicine" json
	And I load the first name "<firstName>"
	And I load the last name "<lastName>"
	And I load the license number "<licenseNumber>"
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the messageId in CloudWatchLogs group "/aws/lambda/pdm-dev-vfy-daqWorkers-IABoardOfMedicine-func"
	And I check for error messages
	And I parse the json response for IOWA Board of medicine
	And I verify the key "data.licenseDetails.issueDate" with the value "<issueDate>"
	And I verify the key "data.licenseDetails.expirationDate" with the value "<expirationDate>"
	And I verify the key "data.licenseDetails.licenseNumber" with the value "<licenseNumber>"
	And I check the disciplinary records to match with "<disciplinaryActionRecords>"
	And I check proof of artifact

Examples:
	| firstName | lastName  | licenseNumber | issueDate           | expirationDate | disciplinaryActionRecords |
	| Fred      | Goldblatt | DO-02020      | Jan 31 1984 12:00AM | 11/01/2024     | 0                         |
	| Sally     | Smith     | R-12632       | 06/08/2022          | 06/30/2024     | 0                         |
	| Kathleen  | Jones     | MD-32008      | Aug 12 1997 12:00AM | 12/01/2022     | 0                         |
	| Abigail   | Scrogum   | R-12485       | 05/20/2022          | 06/30/2025     | 0                         |
	| Kirk      | Smith     | NA            | NA                  | 04/26/2002     | 2                         |


Scenario: User does not exist
	Given I open the "IOWABoardOfMedicine" json
	And I load the first name "aqweds"
	And I load the last name "sdftrtf"
	And I load the license number "sdftgfddsf"
	And I load the messageId
	And I set IABoardOfMedicine message attributes

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the messageId in CloudWatchLogs group "/aws/lambda/pdm-dev-vfy-daqWorkers-IABoardOfMedicine-func"
	And I parse the json response for IOWA Board of medicine
	And I verify the key "destination" with the value "PSV-DAQ | Failures"
	And I verify the key "messageType" with the value "RetrievingDataFromPrimarySource_Failed"

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


#Scenario: Test DB conn
#	Then I connect to psv database cluster




