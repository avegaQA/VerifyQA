#Author: alfonso.vega@symplr.com
#Keywords Summary :
@EventBridge
Feature: EventBridge
  
  Scenario: Check if rule is set
    When I list all rules
    Then I check "pdm-dev-vfy-psvDaq-clock" is created