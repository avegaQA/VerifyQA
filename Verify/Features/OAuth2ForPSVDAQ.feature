#UserStory: https://dev.azure.com/symplr/Provider%20Management/_backlogs/backlog/Gondor/Features/?workitem=555685



Feature: OAuth2 Access Token & Authorization For PSV-DAQ Api Endpoints

Scenario: license-psv-data-requests

	Given I open the "license-psv-data-requests" json in folder "LicenseAPI"
	And I prepare the JSON data
		| key                 | value   |
		| subscribingSystemId | sVerify |

	Given I get the access token for API

	When I "POST" the payload to "https://verify.nonprod.symplr.com/gondordev01/api/license-psv-data-requests"

	Then I parse the API response
	And I verify that JSON response contains
		| key         | value                                                                               |
		| messages[0] | This license was previously enrolled in periodic monitoring for subscribing system: |

#********************************************************************************************************************

Scenario: license-monitoring-enrollments

	Given I open the "license-psv-data-requests" json in folder "LicenseAPI"
	And I prepare the JSON data
		| key                 | value   |
		| subscribingSystemId | sVerify |

	Given I get the access token for API

	When I "POST" the payload to "https://verify.nonprod.symplr.com/gondordev01/api/license-monitoring-enrollments"

	Then I parse the API response
	And I verify that JSON response contains
		| key         | value                                                                                |
		| messages[0] | This license has already been enrolled in periodic monitoring for subscribing system |
