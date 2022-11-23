using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    public class DAM : TestBase
    {
        private AWSContext _awsContext;

        public RestClient client { get; private set; }
        public RestRequest request { get; private set; }

        public String accessToken { get; private set; }

        public DAM(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [Then(@"I get the access token")]
        public async Task ThenIGetTheAccessTokenAsync()
        {
            RestResponse response = await this.GetAccessToken();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            JObject jsonResponse = JObject.Parse(response.Content);

            this.accessToken = jsonResponse["access_token"].ToString();
        }

        [Then(@"I verify the proof of artifact")]
        public async Task ThenIVerifyTheProofOfArtifactAsync()
        {
            RestResponse response = await this.GetProofOfArtifact(this._awsContext.response.SelectToken("data._links.get_proofArtifacts[0].href").ToString());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Dictionary<string, string> responseHeaders = HeadersToDictionary(response.Headers.ToList());

            this.LogAndReport("File name: " + responseHeaders["FileName"]);
            this.LogAndReport("Status code: " + response.StatusCode);
            Assert.IsTrue(responseHeaders["FileName"].Contains(this._awsContext.messsageID));

            this.addLinkToReport(responseHeaders["PresignedUrl"], "Presigned PDF URL (Expires in 30min)");
        }

        [Then(@"I verify the Raw HTML")]
        public async Task ThenIVerifyTheRawHTML()
        {
            RestResponse response = await this.GetProofOfArtifact(this._awsContext.response.SelectToken("data._links.get_primarySourceRawHtml[0].href").ToString());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Dictionary<string, string> responseHeaders = HeadersToDictionary(response.Headers.ToList());

            this.LogAndReport("Status code: " + response.StatusCode);

            this.addLinkToReport(responseHeaders["PresignedUrl"], "Presigned HTML URL (Expires in 30min)");
        }

        [Then(@"I check the disciplinary records to match with ""([^""]*)""")]
        public async Task ThenICheckTeDisciplinaryRecordsToMatchWithAsync(string expected)
        {
            int records = int.Parse(expected);

            var token = this._awsContext.response.SelectToken("data._links.get_disciplinaryActionRecords");

            if (token != null)
            {
                JToken[] recordsData = this._awsContext.response.SelectToken("data._links.get_disciplinaryActionRecords").ToArray();
                Assert.AreEqual(records, recordsData.Length);

                foreach (JToken record in recordsData)
                {
                    RestResponse response = await this.GetProofOfArtifact(record["href"].ToString());

                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    Dictionary<string, string> responseHeaders = HeadersToDictionary(response.Headers.ToList());

                    this.LogAndReport("File name: " + responseHeaders["FileName"]);
                    this.LogAndReport("Status code: " + response.StatusCode);

                    Assert.IsTrue(responseHeaders["FileName"].Contains(this._awsContext.messsageID));

                    this.addLinkToReport(responseHeaders["PresignedUrl"], "Presigned PDF URL (Expires in 30min)");
                }
            }
            else
            {
                Assert.AreEqual(records, 0);
            }
        }


        public async Task<RestResponse> GetAccessToken()
        {
            String PINGONE_PASSWORD = Environment.GetEnvironmentVariable("PINGONE_PASSWORD");
            String PINGONE_USERNAME = Environment.GetEnvironmentVariable("PINGONE_USERNAME");

            client = new RestClient("https://auth.pingone.com");

            request = new RestRequest("6ac11201-e21d-452b-b716-9449c61be923/as/token")
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("scope", "DigitalAssets:Access");

            client.Authenticator = new HttpBasicAuthenticator(PINGONE_USERNAME, PINGONE_PASSWORD);

            return await client.PostAsync(request);

        }

        public Task<RestResponse> GetProofOfArtifact(String pUrl)
        {
            client = new RestClient("http://platformassetsapi-dev.us-west-2.elasticbeanstalk.com");

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.accessToken, "Bearer");

            request = new RestRequest(pUrl);

            return client.HeadAsync(request);

        }

        public Dictionary<string, string> HeadersToDictionary(List<HeaderParameter> responseHeaders)
        {
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            foreach (HeaderParameter header in responseHeaders)
            {
                headerDic.Add(header.Name.ToString(), header.Value.ToString());
            }

            return headerDic;
        }

    }
}
