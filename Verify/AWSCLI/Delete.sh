#!/bin/bash

db=PsvDaqDataStore
schema=public
AWS_SECRETS_NAME=pdm-gondordev01-vfy-psvdaq-database-secret-3
AWS_RDS_NAME=pdm-gondordev01-vfy-psvdaq-database-cluster

AWS_SECRETS_ARN=$(aws secretsmanager describe-secret --secret-id $AWS_SECRETS_NAME | jq '.ARN')
AWS_SECRETS_ARN=${AWS_SECRETS_ARN::-1}
AWS_SECRETS_ARN=${AWS_SECRETS_ARN:1}

AWS_RDS_ARN=$(aws rds describe-db-clusters --db-cluster-identifier $AWS_RDS_NAME | jq '.DBClusters[0]' | jq '.DBClusterArn')
AWS_RDS_ARN=${AWS_RDS_ARN::-1}
AWS_RDS_ARN=${AWS_RDS_ARN:1}

sql="delete from public.\"Enrollment\" where \"CustomerId\"='TestAutomation';"

aws rds-data execute-statement --database $db --resource-arn $AWS_RDS_ARN --schema $schema --secret-arn $AWS_SECRETS_ARN  --sql "$sql"
