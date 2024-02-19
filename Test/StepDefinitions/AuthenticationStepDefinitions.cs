using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using Recipe_Api.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;


namespace Test.StepDefinitions
{
    [Binding]
    public sealed class AuthenticationStepDefinitions
    {
        private Boolean _isAdmin = false;
        private String _username;
        private String _password;
        private RestResponse _response;
        private String _Token;
        private String _url = "https://localhost:7290/api/Authentication/";

        [Given(@"for authentication the recipe API is located at (.*)")]
        public void GivenTheRecipeApiIsLocatedAt(string baseUrl)
        {
            _url = baseUrl;
        }

        [Given(@"a (?:new )?user with username: (.*) and password: (.*)")]
        public void GivenANewUserWithUsernameAndPassword(string username, string password)
        {
            _username = username;
            _password = password;
        }

        [Given(@"wants to be admin")]
        public void GivenWantsToBeAdmin()
        {
            _isAdmin = true;
        }

        [Given("the user already exists")]
        [When(@"the user attempts to register")]
        public async Task WhenTheUserAttemptsToRegister()
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}register")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            var payload = new
            {
                Username = _username,
                Password = _password,
                isAdmin = _isAdmin
            };

            request.AddJsonBody(payload);
            _response = client.Execute(request);
        }

        [When(@"the user attempts to login")]
        public async Task WhenTheUserAttemptsToLogin()
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}login")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            var payload = new
            {
                Username = _username,
                Password = _password,
            };

            request.AddJsonBody(payload);
            _response = client.Execute(request);
        }

        [Then(@"the response status is (.*)")]
        public void ThenTheResponseStatusIs(string expectedStatus)
        {
            var expectedHttpStatusCode = (HttpStatusCode)int.Parse(expectedStatus);
            _response.StatusCode.Should().Be(expectedHttpStatusCode);
        }

        [Then(@"the user should receive a success message")]
        public async Task ThenTheUserShouldReceiveASuccessMessage()
        {
            Response apiResponse = JsonConvert.DeserializeObject<Response>(_response.Content);

            apiResponse.Status.Should().Be("Success");
            apiResponse.Message.Should().Be("User created successfully!");
        }

        [Then(@"the user should receive a key")]
        public async Task ThenTheUserShouldReceiveAKey()
        {
            var responseContent = _response.Content;
            dynamic apiResponse = JObject.Parse(responseContent);

            string token = apiResponse.token;
            token.Should().NotBeNullOrEmpty();
        }
    }
}
