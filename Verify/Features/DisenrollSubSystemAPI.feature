
#dotnet test --filter Category=RunThis
Feature: Disenroll the enrollment for the requesting subscribing system's enrollmentId

@RunThis
@AWSCLI
Scenario: Disenroll the enrollment for the requesting subscribing system's enrollmentId

	Given I open the "license-psv-data-requests" json in folder "LicenseAPI"
	And I prepare the JSON data
		| key                                                           | value          |
		| subscribingSystemId                                           | sVerify        |
		| customerId                                                    | TestAutomation |
	And I set random license number, firstname and lastname

	Given I get the access token for API

	When I "POST" the payload to "https://verify.nonprod.symplr.com/gondordev01/api/license-psv-data-requests"

	Then I parse the API response with a valid response
	And I get the EnrollmentId
	And I verify the "True" status by EnrollmentId

	When I disenroll the subscribing system by the API
	Then I verify the "False" status by EnrollmentId
	
	And I clean the database from any test data

