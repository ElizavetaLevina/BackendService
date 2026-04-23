using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using Moq;

namespace BackendService.Tests
{
    public class TagLogicTests
    {
        private readonly Mock<ITagRepository> _tagRepository;
        private readonly ITagLogic _tagLogic;

        public TagLogicTests()
        {
            _tagRepository = new Mock<ITagRepository>();
            _tagLogic = new TagLogic(_tagRepository.Object);
        }

        [Fact]
        public async Task GetTags_ReturnsListOfTags()
        {
            var fakeDTOs = new List<TagEditDTO> { new TagEditDTO { Id = 1 }, new TagEditDTO { Id = 2 } };

            _tagRepository.Setup(c => c.GetTags(It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTOs);

            var result = await _tagLogic.GetTags();

            Assert.NotNull(result);
            Assert.Equal(fakeDTOs.Count, result.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(20)]
        public async Task GetTagById_ValidId_ReturnsTag(int tagId)
        {
            var fakeDTO = new TagEditDTO { Id = tagId };

            _tagRepository.Setup(c => c.GetTagById(tagId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);

            var result = await _tagLogic.GetTagById(tagId);

            Assert.NotNull(result);
            Assert.Equal(fakeDTO.Id, result.Id);
        }

        [Theory]
        [InlineData(1000)]
        public async Task GetTagById_ValidId_ThrowsNotFoundException(int tagId)
        {
            _tagRepository.Setup(c => c.GetTagById(tagId, It.IsAny<CancellationToken>())).ReturnsAsync((TagEditDTO?)null);

            await Assert.ThrowsAsync<NotFoundException>(async() => await  _tagLogic.GetTagById(tagId));

            _tagRepository.Verify(c => c.GetTagById(tagId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteTag_ValidId_DeletesTag(int tagId)
        {
            _tagRepository.Setup(c => c.DeleteTag(tagId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _tagLogic.DeleteTag(tagId);

            _tagRepository.Verify(c => c.DeleteTag(tagId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(1, "Test")]
        [InlineData(0, "Test")]
        public async Task SaveTag_ValidTag_ReturnsSavedTag(int tagId, string name)
        {
            var fakeDTO = new TagEditDTO { Id = tagId, Name = name };

            _tagRepository.Setup(c => c.SaveTag(fakeDTO, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);

            var result = await _tagLogic.SaveTag(fakeDTO);

            Assert.NotNull(result);
            Assert.Equal(fakeDTO.Id, result.Id);
        }
    }
}
