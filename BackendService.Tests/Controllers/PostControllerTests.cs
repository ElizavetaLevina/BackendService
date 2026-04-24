using BackendService.Common.DTO;
using BackendService.Tests.Factories;
using BackendService.Tests.Helpers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BackendService.Tests.Controllers
{
    [Collection("IntegrationTests")]
    public class PostControllerTests
    {
        private readonly HttpClient _client;

        public PostControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPosts_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("api/Post/list");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetPosts_WithReadRole_Returns200()
        {
            var token = await KeycloakTokenHelper.GetToken("user-test", "user-test-pass");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/Post/list");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreatePost_WithReadRole_Returns403()
        {
            var token = await KeycloakTokenHelper.GetToken("user-test", "user-test-pass");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var post = new PostEditDTO
            {
                Title = "Test",
                TextPost = "Test"
            };

            var content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Post", content);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreatePost_WithEditRole_Returns200()
        {
            var token = await KeycloakTokenHelper.GetToken("user-re-test", "user-re-test-pass");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var post = new PostEditDTO
            {
                Title = "Test",
                TextPost = "Test"
            };

            var content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Post", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
