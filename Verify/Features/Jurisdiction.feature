
#US: https://dev.azure.com/symplr/Provider%20Management/_backlogs/backlog/Gondor/Features/?workitem=576412
@NEW
Feature: Jurisdiction instead of PrimarySource

#********************************************************************************************************************

Scenario: Record a new license through API (license-psv-data-requests)

	Given I open the "license-psv-data-requestsPrimarySource" json in folder "LicenseAPI"
	And I set random license number, firstname and lastname

	Given I get the access token for API

	When I "POST" the payload to "https://verify.nonprod.symplr.com/gondordev01/api/license-psv-data-requests"

	Then I verify the JSON response
		| key         | value                        |
		| messages[0] | A 'jurisdiction' is required |

#********************************************************************************************************************

Scenario: Record a new license through API (license-monitoring-enrollments)

	Given I open the "license-psv-data-requestsPrimarySource" json in folder "LicenseAPI"
	And I set random license number, firstname and lastname

	Given I get the access token for API

	When I "POST" the payload to "https://verify.nonprod.symplr.com/gondordev01/api/license-monitoring-enrollments"

	Then I verify the JSON response
		| key         | value                        |
		| messages[0] | A 'jurisdiction' is required |

#********************************************************************************************************************
