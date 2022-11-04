using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
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

        [Given(@"I prepare the JSON data")]
        public void GivenIPrepareTheJSONData(Table table)
        {
            Dictionary<string, string> jsonValues = this.ToDictionary(table);
            foreach(var value in jsonValues)
            {               
                String val = value.Value.Equals("NA") ? "" : value.Value;
                this.LogAndReport("Replacing " + value.Key + " with " + val);
                this._awsContext.payload.SelectToken(value.Key).Replace(val);
            }

            Console.WriteLine(this._awsContext.payload.ToString());
        }

        [Then(@"I verify the JSON response")]
        public void ThenIVerifyTheJSONResponse(Table table)
        {
            Dictionary<string, string> expectedValues = this.ToDictionary(table);

            foreach (var expectedValue in expectedValues)
            {
                String value = expectedValue.Value;
                if (value.ToLower().Replace(" ", "").Equals("na")) value = "";

                String real = value.ToLower().Replace(" ", "");
                String expected = this._awsContext.response.SelectToken(expectedValue.Key).ToString().ToLower().Replace(" ", "");
                this.LogAndReport("Expected value in "+ expectedValue.Key + ": " + value);
                this.LogAndReport("Real value: " + this._awsContext.response.SelectToken(expectedValue.Key));

                Assert.AreEqual(real, expected);

            }
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
            if (license.ToLower().Replace(" ", "").Equals("na")) license = "";
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
            String resp = this._awsContext.response["message"].ToString();
            this.LogAndReport(resp);
            this._awsContext.response = JObject.Parse(resp);
        }

        [Then(@"I check for error messages")]
        public void ThenICheckForErrorMessages()
        {
            String resp = this._awsContext.response["message"].ToString();           
            if (resp.ToLower().Contains("error") || resp.ToLower().Contains("fail") || resp.ToLower().Contains("exception"))
            {
                this.LogAndReport(resp);
                Assert.Fail("Response contains error message");
            }
                
        }

        [Given(@"I load the destination to ""([^""]*)""")]
        public void GivenILoadTheDestinationTo(string destination)
        {
            this._awsContext.payload["destination"] = destination;
        }



        [Then(@"I check the disciplinary records to match with ""([^""]*)""")]
        public void ThenICheckTeDisciplinaryRecordsToMatchWith(string expected)
        {
            JArray items = (JArray)this._awsContext.response["data"]["_links"]["get_disciplinaryActionRecords"];
            int length = items.Count;

            this.LogAndReport("Expected value: " + expected);
            this.LogAndReport("Real value: " + length);

            Assert.AreEqual(int.Parse(expected), length);
        }



        [Then(@"I check proof of artifact")]
        public void ThenICheckProofOfArtifact()
        {
            String path = this._awsContext.response.SelectToken("data._links.get_proofArtifact.href").ToString();
            this.addLinkToReport(path, "Click here to see PDF");
        }

        [Then(@"I connect to psv database cluster")]
        public void ThenIConnectToPsvDatabaseCluster()
        {
            this._awsContext.RDSClient = new AWSHandlers.RDSHandler();
            this._awsContext.RDSClient.connectToDatabase("pdm-dev-vfy-psvdaq-database-cluster.cluster-cgwo3ls8uous.us-east-2.rds.amazonaws.com",
                5432,
                "psvdaq_usr",
                "PsvDaqDataStore");
            this._awsContext.RDSClient.selectFromTable("PsvDataRetrievalResult");
            this._awsContext.RDSClient.closeClient();
        }




    }
}
