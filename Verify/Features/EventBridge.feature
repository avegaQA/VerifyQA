#Author: alfonso.vega@symplr.com
#Keywords Summary :
@RunThis
Feature: EventBridge
  
  Scenario: Check if rule is set
    Given I log in into AWS EventBridge
    When I list all rules
    Then I check "pdm-dev-vfy-psvDaq-clock" is created
    Then I close the AWS EventBridge