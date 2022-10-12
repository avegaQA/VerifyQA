using Amazon.SimpleNotificationService.Model;
using System;
using TechTalk.SpecFlow;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    public class IOWABoardOfMedicineStepDefinitions : TestBase
    {
        private AWSContext _awsContext;

        public IOWABoardOfMedicineStepDefinitions(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [Given(@"I load the first name ""([^""]*)""")]
        public void GivenILoadTheFirstName(string name)
        {
            this._awsContext.payload["data"]["searchAttributes"]["individualNames"][0]["firstName"] = name;
        }

        [Given(@"I load the last name ""([^""]*)""")]
        public void GivenILoadTheLastName(string lastName)
        {
            this._awsContext.payload["data"]["searchAttributes"]["individualNames"][0]["lastName"] = lastName;
        }

        [Given(@"I load the license number ""([^""]*)""")]
        public void GivenILoadTheLicenseNumber(string license)
        {
            this._awsContext.payload["data"]["searchAttributes"]["licenseNumber"] = license;
        }

        [Given(@"I set IABoardOfMedicine message attributes")]
        public void GivenISetIABoardOfMedicineMessageAttributes()
        {
            this._awsContext.snsMessageAttributes.Add("message_destinations", new MessageAttributeValue()
            {
                DataType = "String.Array",
                StringValue = "[\"IABoardOfMedicine\"]"
            });
        }

    }
}
