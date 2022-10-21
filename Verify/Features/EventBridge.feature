#Author: alfonso.vega@symplr.com
#Keywords Summary :
@EventBridge
Feature: EventBridge

Scenario Outline: Check if rule is set
	When I list all rules
	Then I check <ruleName> is created

Examples:
	| ruleName                   |
	| "pdm-dev-vfy-psvDaq-clock" |
	| "pdm-ftr-vfy-psvDaq-clock" |

