@SNS
@CloudWatchLogs
Feature: IOWA Board of Medicine
  
Scenario Outline: Search by name
	Given I open the "IOWABoardOfMedicine" json
	And I load the first name "<firstName>"
	And I load the last name "<lastName>"
	And I load the license number "<licenseNumber>"
	And I load the messageId
	And I set IABoardOfMedicine message attributes 

	When I publish the json to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn

	Then I look for the messageId in CloudWatchLogs group "/aws/lambda/pdm-dev-vfy-daqWorkers-IABoardOfMedicine-func"
	And I parse the json response for IOWA Board of medicine
	And I verify the key "licenseDetails.issueDate" with the value "<issueDate>"
	And I verify the key "licenseDetails.expirationDate" with the value "<expirationDate>"
	And I verify the key "licenseDetails.licenseNumber" with the value "<licenseNumber>"
	And I check proof of artifact

Examples:
	| firstName | lastName  | licenseNumber | issueDate           | expirationDate |
	| Fred      | Goldblatt | DO-02020      | Jan 31 1984 12:00AM | 11/01/2024     |




