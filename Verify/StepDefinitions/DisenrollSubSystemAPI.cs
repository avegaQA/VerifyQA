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

        public DisenrollSubSystemAPI(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [Then(@"I get the EnrollmentId")]
        public void ThenIGetTheEnrollmentId()
        {
            this._awsContext.awscliHandler.SetFileName("database_test.sh");
            this._awsContext.awscliHandler.SetQueryParams("Enrollment", " EnrollmentId"," 2d1fea1f-91d8-41a9-96dd-5c309390cc47");

            String dbResult = this._awsContext.awscliHandler.RunQuery();

            this.LogAndReport(dbResult);
        }

        [Then(@"I get the status by EnrollmentId")]
        public void ThenIGetTheStatusByEnrollmentId()
        {
            
        }


    }
}
