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
            RestResponse response = await this.SendPayload(method, url);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            this.LogAndReport("Expected code: "+ HttpStatusCode.UnprocessableEntity);
            this.LogAndReport("Real code: " + response.StatusCode);
            this._awsContext.response = JObject.Parse(response.Content.ToString());

        }

        [Then(@"I parse the API response")]
        public void ThenIParseTheAPIResponse()
        {
            this.LogAndReport(this._awsContext.response.ToString());
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

    }
}
