Feature: DisenrollSubSystemAPI

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
	And I get the status by EnrollmentId

