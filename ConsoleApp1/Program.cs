using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        private static Boolean _isAdmin = false;
        private static RestResponse _response;
        private static String _token;
        static void Main(string[] args)
        {
            GivenIAmAUser();
            WhenIRequestAListOfCategories();
            WhenIRequestTheCategoryWithId();
            WhenICreateANewCategoryWithName("name");


            Console.WriteLine("press key...");
            Console.ReadKey(true);
        }

        public static void GivenIAmAUser(String username = "TiboAdmin", String password = "Tibo123!")
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: "https://localhost:7290/api/Authentication/login")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            var payload = new
            {
                Username = username,
                Password = password,
            };

            request.AddJsonBody(payload);
            _response = client.Execute(request);
            var responseContent = _response.Content;
            dynamic apiResponse = JObject.Parse(responseContent);

            _token = apiResponse.token;
        }

        public static Task WhenIRequestAListOfCategories()
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: "https://localhost:7290/api/Category")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Authorization", $"Bearer {_token}");
            _response = client.Execute(request);

            var test = _response.Content;

            return null;
        }

        public static Task WhenIRequestTheCategoryWithId(int id = 1)
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"https://localhost:7290/api/Category/{id}")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Authorization", $"Bearer {_token}");
            _response = client.Execute(request);

            var test = _response.Content;
            //Categories apiResponse = JsonConvert.DeserializeObject<Categories>(_response.Content);
            return null;
        }

        public static Task WhenICreateANewCategoryWithName(string name)
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: "https://localhost:7290/api/Category")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            var payload = new
            {
                Name = "fruit",
            };

            request.AddJsonBody(payload);
            _response = client.Execute(request);

            var test = _response.Content;

            return null;
        }
    }
}
