using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Verify.Context;

namespace Verify.StepDefinitions
{
    [Binding]
    public class OAuth2ForPSVDAQ : TestBase
    {
        private AWSContext _awsContext;
        public RestClient client { get; private set; }
        public RestRequest request { get; private set; }
        public String accessToken { get; private set; }

        public RestResponse restResponse { get; private set; }

        public OAuth2ForPSVDAQ(AWSContext awsContext)
        {
            this._awsContext = awsContext;
        }

        [Given(@"I get the access token for API")]
        public async Task GivenIGetTheAccessTokenForAPI()
        {
            RestResponse response = await this.GetAccessTokenAPI();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            JObject jsonResponse = JObject.Parse(response.Content);

            this.accessToken = jsonResponse["access_token"].ToString();
        }

        [When(@"I ""([^""]*)"" the payload to ""([^""]*)""")]
        public async Task WhenIThePayloadToAsync(string method, string url)
        {
            this.restResponse = await this.SendPayload(method, url);
            this._awsContext.response = JObject.Parse(this.restResponse.Content.ToString());

        }

        [Then(@"I parse the API response")]
        public void ThenIParseTheAPIResponse()
        {
            this.LogAndReport(this._awsContext.response.ToString());
            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, this.restResponse.StatusCode);

            this.LogAndReport("Expected code: " + HttpStatusCode.UnprocessableEntity);
            this.LogAndReport("Real code: " + this.restResponse.StatusCode);
            
        }

        [Then(@"I parse the API response with a valid response")]
        public void ThenIParseTheAPIResponseWithAValidResponse()
        {
            this.LogAndReport(this._awsContext.response.ToString());
            Assert.AreEqual(HttpStatusCode.Accepted, this.restResponse.StatusCode);

            this.LogAndReport("Expected code: " + HttpStatusCode.Accepted);
            this.LogAndReport("Real code: " + this.restResponse.StatusCode);
        }

        [Given(@"I set random license number, firstname and lastname")]
        public void GivenISetRandomLicenseNumberFirstnameAndLastname()
        {
            Random rnd = new Random();
            int num = rnd.Next(1000,9999);

            String randString = "TestAuto" + num;

            this.LogAndReport("Replacing searchAttributes.licenseDetails.licenseNumber with " + randString);
            this._awsContext.payload.SelectToken("searchAttributes.licenseDetails.licenseNumber").Replace(randString);

            this.LogAndReport("Replacing searchAttributes.providerDetails.individualNames[0].firstName with " + randString);
            this._awsContext.payload.SelectToken("searchAttributes.providerDetails.individualNames[0].firstName").Replace(randString);

            this.LogAndReport("Replacing searchAttributes.providerDetails.individualNames[0].lastName with " + randString);
            this._awsContext.payload.SelectToken("searchAttributes.providerDetails.individualNames[0].lastName").Replace(randString);
        }

        [When(@"I disenroll the subscribing system by the API")]
        public async Task WhenIDisenrollTheSubscribingSystemByTheAPI()
        {
            String url;
            
            url = "https://verify.nonprod.symplr.com/gondordev01/api/license-monitoring-enrollments/" + this._awsContext.response["enrollmentId"].ToString();
            this.restResponse = await this.CallAPI("DELETE", url);
            this.LogAndReport("Response Code: "+this.restResponse.StatusCode.ToString());
            Assert.AreEqual(HttpStatusCode.OK, this.restResponse.StatusCode);
        }




        public async Task<RestResponse> GetAccessTokenAPI()
        {
            String PINGONE_PASSWORD = Environment.GetEnvironmentVariable("PINGONE_PASSWORD_API");
            String PINGONE_USERNAME = Environment.GetEnvironmentVariable("PINGONE_USERNAME_API");

            client = new RestClient("https://auth.pingone.com");

            request = new RestRequest("6ac11201-e21d-452b-b716-9449c61be923/as/token")
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("scope", "verify:psvdaq:api:access verify:subscribing-system-id:qa");

            client.Authenticator = new HttpBasicAuthenticator(PINGONE_USERNAME, PINGONE_PASSWORD);

            return await client.PostAsync(request);

        }

        public Task<RestResponse> SendPayload(string method, string url)
        {
            var options = new RestClientOptions(url)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,

            };
            client = new RestClient(options);

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.accessToken, "Bearer");

            request = new RestRequest(url);
            request.AddStringBody(this._awsContext.payload.ToString(), DataFormat.Json);
            this.LogAndReport(this._awsContext.payload.ToString());

            switch (method.ToLower())
            {
                case "post":
                    return client.ExecutePostAsync(request);

                case "get":
                    return client.ExecuteGetAsync(request);

                default:
                    return null;
            }
        }

        public Task<RestResponse> CallAPI(string method, string url)
        {
            var options = new RestClientOptions(url)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,

            };
            client = new RestClient(options);

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.accessToken, "Bearer");

            request = new RestRequest(url);

            switch (method.ToLower())
            {
                case "post":
                    return client.ExecutePostAsync(request);

                case "get":
                    return client.ExecuteGetAsync(request);

                case "delete":
                    return client.DeleteAsync(request);

                default:
                    return null;
            }
        }

    }
}
