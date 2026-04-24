using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using Moq;

namespace BackendService.Tests.Logics
{
    public class PostLogicTests
    {
        private readonly Mock<IPostRepository> _postRepository;

        private readonly IPostLogic _postLogic;

        public PostLogicTests()
        {
            _postRepository = new Mock<IPostRepository>();
            _postLogic = new PostLogic(_postRepository.Object);
        }

        [Fact]
        public async Task GetPosts_ReturnsListOfPosts()
        {
            var fakeDTOs = new List<PostDTO> { new PostDTO { Id = 1 }, new PostDTO { Id = 2 } };
            _postRepository.Setup(c => c.GetPosts(It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTOs);

            var result = await _postLogic.GetPosts();

            Assert.NotNull(result);
            Assert.Equal(fakeDTOs.Count, result.Count);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPostById_ValidId_ReturnsPost(int postId)
        {
            var fakeDTO = new PostDTO { Id = postId };
            _postRepository.Setup(c => c.GetPostById(postId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);

            var result = await _postLogic.GetPostById(postId);

            Assert.NotNull(result);
            Assert.Equal(postId, result.Id);
        }

        [Theory]
        [InlineData(1, "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111")]
        public async Task DeletePost_UserIsOwner_DeletesPost(int postId, string userIdString, string ownerUserId)
        {
            var userId = Guid.Parse(userIdString);
            var ownerId = Guid.Parse(ownerUserId);

            _postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);

            _postRepository.Setup(c => c.DeletePost(postId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _postLogic.DeletePost(postId, userId);

            _postRepository.Verify(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>()), Times.Once);
            _postRepository.Verify(c => c.DeletePost(postId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(3, "11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222")]
        public async Task DeletePost_UserIsNotOwner_ThrowsForbiddenException(int postId, string userIdString, string ownerIdString)
        {
            var userId = Guid.Parse(userIdString);
            var ownerId = Guid.Parse(ownerIdString);

            _postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);

            await Assert.ThrowsAsync<ForbiddenException>(async () => await _postLogic.DeletePost(postId, userId));

            _postRepository.Verify(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>()), Times.Once);
            _postRepository.Verify(c => c.DeletePost(postId, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(1, "Test", "Test", "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111")]
        [InlineData(0, "Test", "Test", "22222222-2222-2222-2222-222222222222", null)]
        public async Task SavePost_ValidPost_ReturnsSavedPost(int id, string title, string textPost, string userIdString, string? ownerIdString)
        {
            var fakeDTO = new PostEditDTO { Id = id, Title = title, TextPost = textPost };
            var userId = Guid.Parse(userIdString);

            _postRepository.Setup(c => c.SavePost(fakeDTO, userId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);

            if (id > 0)
                _postRepository.Setup(c => c.GetUserIdByPostId(id, It.IsAny<CancellationToken>())).ReturnsAsync(Guid.Parse(ownerIdString));

            var result = await _postLogic.SavePost(fakeDTO, userId);

            Assert.NotNull(result);
            Assert.Equal(fakeDTO.Id, result.Id);
        }
    }
}