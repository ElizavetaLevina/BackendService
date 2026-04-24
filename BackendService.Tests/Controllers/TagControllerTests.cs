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
    public class TagControllerTests
    {
        private readonly HttpClient _client;

        public TagControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateTag_WithAdminRole_Returns200()
        {
            var token = await KeycloakTokenHelper.GetToken("admin-test", "admin-test-pass");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var tag = new TagEditDTO
            {
                Name = $"Test {Guid.NewGuid()}"
            };

            var content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Tag", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTag_WithEditRole_Returns403()
        {
            var token = await KeycloakTokenHelper.GetToken("user-re-test", "user-re-test-pass");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("api/Tag/5");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
