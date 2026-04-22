using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using Moq;

namespace BackendService.Tests
{
    public class LogicTest
    {
        private readonly Mock<IPostRepository> _postRepository;

        private readonly IPostLogic _postLogic;

        public LogicTest()
        {
            _postRepository = new Mock<IPostRepository>();
            _postLogic = new PostLogic(_postRepository.Object);
        }

        #region PostLogic

        [Fact]
        public async Task GetPostsTest()
        {
            var fakeDTOs = new List<PostDTO> { new PostDTO { Id = 1}, new PostDTO { Id = 2 } };
            _postRepository.Setup(c => c.GetPosts(It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTOs);

            var result = await _postLogic.GetPosts();

            Assert.NotNull(result);
            Assert.Equal(fakeDTOs.Count, result.Count);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPostByIdSuccessTest(int postId)
        {
            var fakeDTO = new PostDTO { Id = postId };
            _postRepository.Setup(c => c.GetPostById(postId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);

            var result = await _postLogic.GetPostById(postId);

            Assert.NotNull(result);
            Assert.Equal(postId, result.Id);
        }

        [Theory]
        [InlineData(1000)]
        public async Task GetPostByIdNotFoundTest(int postId)
        {
            _postRepository.Setup(c => c.GetPostById(postId, It.IsAny<CancellationToken>())).ReturnsAsync((PostDTO)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _postLogic.GetPostById(postId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetPostByIdInvalidIdTest(int postId)
        {          
            await Assert.ThrowsAsync<ValidationException>(async () => await _postLogic.GetPostById(postId));

            _postRepository.Verify(c => c.GetPostById(postId, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(1, "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111")]
        public async Task DeletePostSuccessTest(int postId, string userIdString, string ownerUserId)
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
        public async Task DeletePostNotSuccessTest(int postId, string userIdString, string ownerUserId)
        {
            var userId = Guid.Parse(userIdString);
            var ownerId = Guid.Parse(ownerUserId);

            _postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);

            await Assert.ThrowsAsync<ForbiddenException>(async () => await _postLogic.DeletePost(postId, userId));

            _postRepository.Verify(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>()), Times.Once);
            _postRepository.Verify(c => c.DeletePost(postId, It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion
    }
}