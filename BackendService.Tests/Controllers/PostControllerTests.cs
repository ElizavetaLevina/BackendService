using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using BackendService.DAL.Models;
using BackendService.Tests.Factories;
using BackendService.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public PostControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _dbContext = factory._dbContext;
            _factory = factory;
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

            var post = new PostPendingEditDTO
            {
                Title = "Test",
                TextPost = "Test"
            };

            var content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Post", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = JsonSerializer.Deserialize<PostPendingEditDTO>(await response.Content.ReadAsStringAsync(), _jsonOptions);
            Assert.NotNull(result);
            Assert.Null(result.PostId);
        }

        [Fact]
        public async Task RejectPost_StatusBecomesRejected()
        {
            var token = await KeycloakTokenHelper.GetToken("user-re-test", "user-re-test-pass");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var post = new PostPendingEditDTO
            {
                Title = "Test",
                TextPost = "Test"
            };

            var content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Post", content);
            var created = JsonSerializer.Deserialize<PostPendingEditDTO>(await response.Content.ReadAsStringAsync(), _jsonOptions);
            var pendingId = created.Id;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using var scope = _factory.Services.CreateScope();
            var logic = scope.ServiceProvider.GetRequiredService<IPostPendingLogic>();
            var expEvent = new PostModeratedEvent { PendingId = pendingId, Status = StatusModerationEnum.Rejected, RejectionReason = "Test reason" };

            await logic.RejectPost(expEvent);

            var result = await _dbContext.PostsPending.FindAsync(pendingId);
            Assert.NotNull(result);
            Assert.Equal(StatusModerationEnum.Rejected, result.Status);
        }

        [Fact]
        public async Task ApprovePost_DeletesPendingAndCreatesPost()
        {
            var token = await KeycloakTokenHelper.GetToken("user-re-test", "user-re-test-pass");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var textPost = $"Test_{Guid.NewGuid()}";
            var post = new PostPendingEditDTO
            {
                Title = "Test",
                TextPost = textPost
            };

            var content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Post", content);
            var created = JsonSerializer.Deserialize<PostPendingEditDTO>(await response.Content.ReadAsStringAsync(), _jsonOptions);
            var pendingId = created.Id;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using var scope = _factory.Services.CreateScope();
            var logic = scope.ServiceProvider.GetRequiredService<IPostPendingLogic>();
            var expEvent = new PostModeratedEvent { PendingId = pendingId, Status = StatusModerationEnum.Approved };

            await logic.ApprovePost(pendingId);

            var deletedPendingPost = await _dbContext.PostsPending.FindAsync(pendingId);
            Assert.Null(deletedPendingPost);

            var approvedPost = await _dbContext.Posts.FirstOrDefaultAsync(c => c.TextPost == textPost);
            Assert.NotNull(approvedPost);
            Assert.Equal(textPost, approvedPost.TextPost);
            Assert.Equal("Test", approvedPost.Title);
        }

    }
}
