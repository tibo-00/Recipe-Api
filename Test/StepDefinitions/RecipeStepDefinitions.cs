using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Recipe_Api.Data;
using Recipe_Api.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.StepDefinitions
{
    [Binding]
    public class RecipeStepDefinitions
    {
        private RestResponse _response;
        private String _token;
        private Recipes _currentRecipe;
        private String _url;

        [Given(@"the recipe API is located at (.*)")]
        public void GivenTheRecipeApiIsLocatedAt(string baseUrl)
        {
            _url = baseUrl;
        }

        [Given(@"I am an logged in user with username: (.*) and Password: (.*)")]
        public void GivenIAmAUser(String username, String password)
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}Authentication/login")
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
        

        [When(@"I request a list of (.*)")]
        public async Task WhenIRequestAList(string listName)
        {
            if (listName.Equals("recipes"))
            {
                listName = "Recipe";
            } else if (listName.Equals("categories"))
            {
                listName = "Category";
            }
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}{listName}")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Authorization", $"Bearer {_token}");
            _response = client.Execute(request);
        }

        [When(@"I delete that recipe with it's (?: non-existent )?id")]
        public async Task WhenIDeleteRecipeWithId()
        {
            
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}Recipe/{_currentRecipe.Id}")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Delete;
            request.AddHeader("Authorization", $"Bearer {_token}");
            _response = client.Execute(request);
        }

        [When(@"I request a (.*) with (?:a non-existent )?id (\d+)")]
        public async Task WhenIRequestWithId(string listName, int id)
        {
            if (listName.Equals("recipe"))
            {
                listName = "Recipe";
            }
            else if (listName.Equals("category"))
            {
                listName = "Category";
            }
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}{listName}/{id}")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Authorization", $"Bearer {_token}");
            _response = client.Execute(request);
        }

        [When(@"I create a new category with name: (.*)")]
        public async Task WhenICreateANewCategoryWithName(string name)
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}Category")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            var payload = new
            {
                Name = name,
            };

            request.AddJsonBody(payload);
            request.AddHeader("Authorization", $"Bearer {_token}");
            _response = client.Execute(request);
        }

        [Given(@"I create a new recipe")]
        [When(@"I create a new recipe")]
        public async Task WhenICreateANewRecipe()
        {
            RestClient client = new RestClient(new RestClientOptions(baseUrl: $"{_url}Recipe")
            {
                MaxTimeout = 10000
            });
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            var payload = new
            {
                title = "Tiramisu",
                difficulty = 2,
                time = 35,
                categoryId = 5,
                ingredients = new[]
                {
                    new { name = "eieren", quantity = 3, unit = "stuk" },
                    new { name = "suiker", quantity = 100, unit = "g" },
                    new { name = "mascarpone", quantity = 500, unit = "g" },
                    new { name = "amaretto", quantity = 1, unit = "scheutjes" }, 
                    new { name = "sterke koffie", quantity = 1, unit = "kopje" },
                    new { name = "lange vingers", quantity = 300, unit = "g" },
                    new { name = "cacaopoeder", quantity = 3, unit = "eetlepels" }
                }
            };

            request.AddJsonBody(payload);
            request.AddHeader("Authorization", $"Bearer {_token}");
            _response = client.Execute(request);
            _currentRecipe = JsonConvert.DeserializeObject<Recipes>(_response.Content);
        }

        [Then(@"I should receive a list of all categories including (.*), (.*), (.*), (.*) and (.*)")]
        public void ThenIShouldReceiveAListOfAllCategories(string category1, string category2, string category3, string category4, string category5)
        {
            List<Categories> apiResponse = JsonConvert.DeserializeObject<List<Categories>>(_response.Content);

            apiResponse.Should().NotBeNull("because the response should contain a list of categories");
            apiResponse.Should().NotBeEmpty("because the response should include at least one category");

            var expectedCategories = new List<string> { category1, category2, category3, category4, category5 };
            foreach (var expectedCategory in expectedCategories)
            {
                apiResponse.Any(category => category.Name == expectedCategory).Should().BeTrue($"because the category list should include '{expectedCategory}'");
            }
        }

        [Then(@"I should receive the recipe details")]
        public void ThenIShouldReceiveTherecipeDetails()
        {
            Recipes apiResponse = JsonConvert.DeserializeObject<Recipes>(_response.Content);
            apiResponse.Should().NotBeNull("because the recipe should be provided in the response");
        }

        [Then(@"I should receive the recipe details with id (.*)")]
        public void ThenIShouldReceiveTherecipeDetails(int id)
        {
            Recipes apiResponse = JsonConvert.DeserializeObject<Recipes>(_response.Content);
            apiResponse.Should().NotBeNull("because the recipe should be provided in the response");
            apiResponse.Id.Should().Be(id, "because the Id has to match");
        }

        [Then(@"I should receive a list of all recipes")]
        public void ThenIShouldReceiveAListOfAllRecipes()
        {
            List<Recipes> apiResponse = JsonConvert.DeserializeObject<List<Recipes>>(_response.Content);

            apiResponse.Should().NotBeNull("because the response should contain a list of categories");
            apiResponse.Should().NotBeEmpty("because the response should include at least one category");

            foreach (var category in apiResponse)
            {
                category.Id.Should().BeGreaterThan(0, "because each recipe should have a valid ID");
                category.Title.Should().NotBeNullOrEmpty("because each recipe should have a title");
            }
        }

        [Then(@"I should receive the category details")]
        public void ThenIShouldReceiveTheCategoryDetails()
        {
            Categories apiResponse = JsonConvert.DeserializeObject<Categories>(_response.Content);
            apiResponse.Name.Should().NotBeNull("because the category name should be provided in the response");
        }


        [Then(@"I should receive a not found response")]
        public void ThenIShouldReceiveANotFoundResponse()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Then(@"I should receive a success response")]
        public void ThenIShouldReceiveASuccess()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"I should receive a created response")]
        public void ThenIShouldReceiveACreatedResponse()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Then(@"I should receive a no content response")]
        public void ThenIShouldReceiveANoContentResponse()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
