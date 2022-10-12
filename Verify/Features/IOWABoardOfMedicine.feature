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

Examples:
	| firstName | lastName  | licenseNumber |
	| Fred      | Goldblatt | DO-02020      |




