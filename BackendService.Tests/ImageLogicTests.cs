using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BackendService.Tests
{
    public class ImageLogicTests
    {
        private readonly Mock<IImageRepository> _imageRepository;
        private readonly Mock<IPostRepository> _postRepository;
        private readonly IImageLogic _imageLogic;

        public ImageLogicTests()
        {
            _imageRepository = new Mock<IImageRepository>();
            _postRepository = new Mock<IPostRepository>();
            _imageLogic = new ImageLogic(_imageRepository.Object, _postRepository.Object);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPostImages_ValidId_ReturnsListOfImages(int postId)
        {
            var fakeDTOs = new List<ImageViewDTO> { new ImageViewDTO { Id = 1 }, new ImageViewDTO { Id = 3 } };
            _imageRepository.Setup(c => c.GetPostImages(postId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTOs);

            var result = await _imageLogic.GetPostImages(postId);

            Assert.NotNull(result);
            Assert.Equal(fakeDTOs.Count, result.Count);
        }

        [Theory]
        [InlineData(1, 2, "22222222-2222-2222-2222-222222222222", "22222222-2222-2222-2222-222222222222")]
        public async Task DeleteImage_UserIsOwner_DeletesImage(int imageId, int postId, string userIdString, string ownerIdString)
        {
            var userId = Guid.Parse(userIdString);
            var ownerId = Guid.Parse(ownerIdString);

            _imageRepository.Setup(c => c.GetPostIdByImageId(imageId, It.IsAny<CancellationToken>())).ReturnsAsync(postId);
            _postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);
            _imageRepository.Setup(c => c.DeleteImage(imageId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _imageLogic.DeleteImage(imageId, userId);

            _imageRepository.Verify(c => c.GetPostIdByImageId(imageId, It.IsAny<CancellationToken>()), Times.Once);
            _postRepository.Verify(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>()), Times.Once);
            _imageRepository.Verify(c => c.DeleteImage(imageId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteImage_ImageNotFound_ThrowsNotFoundException(int imageId)
        {
            _imageRepository.Setup(c => c.GetPostIdByImageId(imageId, It.IsAny<CancellationToken>())).ReturnsAsync((int?)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _imageLogic.DeleteImage(imageId, Guid.NewGuid()));

            _imageRepository.Verify(c => c.DeleteImage(imageId, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task SaveImage_ValidImage_ReturnsSavedImage(int postId)
        {
            var image = new FormFile(new MemoryStream(), 0, 100, "file", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            var userId = Guid.NewGuid();

            _postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(userId);

            _imageRepository.Setup(c => c.SaveImage(It.IsAny<byte[]>(), postId, It.IsAny<CancellationToken>())).ReturnsAsync(10);

            var result = await _imageLogic.SaveImage(image, postId, userId);

            Assert.Equal(10, result);
        }

        [Theory]
        [InlineData(1, 0, "test.jpg", "image/jpeg")]
        [InlineData(1, 11 * 1024 * 1024, "test.jpg", "image/jpeg")]
        [InlineData(1, 130, "test.pdf", "application/pdf")]
        public async Task SaveImage_InvalidImage_ThrowsValidationException(int postId, int fileSize, string fileName, string contentType)
        {
            var userId = Guid.NewGuid();
            var image = new FormFile(new MemoryStream(), 0, fileSize, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            _postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(userId);

            await Assert.ThrowsAsync<ValidationException>(async () => await _imageLogic.SaveImage(image, postId, userId));
        }
    }
}
