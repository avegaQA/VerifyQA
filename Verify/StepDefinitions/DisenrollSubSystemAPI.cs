using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    internal class DisenrollSubSystemAPI : TestBase
    {
        private AWSContext _awsContext;

        public String enrollmentId;

        public DisenrollSubSystemAPI(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [Then(@"I get the EnrollmentId")]
        public void ThenIGetTheEnrollmentId()
        {
            this.enrollmentId = this._awsContext.response["enrollmentId"].ToString();

            this.LogAndReport("EnrollmentId: " + this.enrollmentId);
            
        }

        [Then(@"I verify the ""([^""]*)"" status by EnrollmentId")]
        public void ThenIVerifyTheStatusByEnrollmentId(string expectedStatus)
        {
            String isActive;
            this._awsContext.awscliHandler.initProcess();
            this._awsContext.awscliHandler.SetFileName("Select.sh");
            this._awsContext.awscliHandler.SetQueryParams("Enrollment", " EnrollmentId", this.enrollmentId);

            JObject dbResult = JObject.Parse(this._awsContext.awscliHandler.RunQuery());
            isActive = dbResult.SelectToken("records[0][8].booleanValue").ToString();

            this.LogAndReport(dbResult.ToString());
            this.LogAndReport("Active: " + isActive);

            if (expectedStatus.Equals("True"))
            {
                Assert.True(bool.Parse(isActive));
            }
            else
            {
                Assert.False(bool.Parse(isActive));
            }           
        }

        [Then(@"I clean the database from any test data")]
        public void ThenICleanTheDatabaseFromAnyTestData()
        {
            this._awsContext.awscliHandler.initProcess();
            this._awsContext.awscliHandler.SetFileName("Delete.sh");

            this._awsContext.awscliHandler.RunQuery();
        }


    }
}
