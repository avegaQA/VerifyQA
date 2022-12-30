Feature: SNS infrastructure check

A short summary of the feature

@SNS

Scenario Outline: Check if topic is set
	When I list all topics
	Then I check <topicName> is available

Examples:
	| topicName                             |
	| "pdm-dev-vfy-p1-psvDaqRequests-topic" |
	| "pdm-dev-vfy-p1-psvDaqResults-topic"  |
	| "pdm-dev-vfy-psvDaqRequests-topic"    |
	| "pdm-dev-vfy-psvDaqResults-topic"     |
	#| "pdm-ftr-vfy-p1-psvDaqRequests-topic" |
	#| "pdm-ftr-vfy-p1-psvDaqResults-topic"  |
	#| "pdm-ftr-vfy-psvDaqRequests-topic"    |
	#| "pdm-ftr-vfy-psvDaqResults-topic"     |


#@SQS
#Scenario: Test SQS
#	When I list all messages in queue
