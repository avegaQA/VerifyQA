
@SNS
@CloudWatchLogs
@ignore
@Ignore
Feature: Retrieve_LicensePSVData
  
  Scenario: Retrieve_LicensePSVData
    When I publish the payload to the "arn:aws:sns:us-east-2:379493731719:pdm-dev-vfy-psvDaqRequests-topic" arn
    Then I verify the Cloudwatch logs
    