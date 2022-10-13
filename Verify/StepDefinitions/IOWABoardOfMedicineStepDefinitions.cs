using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json.Linq;
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

        [Then(@"I parse the json response for IOWA Board of medicine")]
        public void ThenIParseTheJsonResponseForIOWABoardOfMedicine()
        {
            String resp = this._awsContext.response["@m"].ToString().Replace("PSV result parsed: ", "") + "}";
            this._awsContext.response = JObject.Parse(resp);
            if (AWSContext.troubleShootReports) this.LogAndReport(this._awsContext.response.ToString());
        }

        [Then(@"I check proof of artifact")]
        public void ThenICheckProofOfArtifact()
        {
            String path = this._awsContext.response.SelectToken("_links.get_proofArtifact.href").ToString();
            this.addLinkToReport(path, "Click here to see PDF");
        }



    }
}
