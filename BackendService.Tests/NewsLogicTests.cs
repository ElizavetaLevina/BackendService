using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using Moq;

namespace BackendService.Tests
{
    public class NewsLogicTests
    {
        private readonly Mock<INewsRepository> _newsRepository;
        private readonly INewsLogic _newsLogic;

        public NewsLogicTests()
        {
            _newsRepository = new Mock<INewsRepository>();
            _newsLogic = new NewsLogic(_newsRepository.Object);
        }

        [Theory]
        [InlineData("us", null, null, null, null, null)]
        [InlineData(null, "health", null, null, null, null)]
        [InlineData(null, null, "fox-news", null, null, null)]
        [InlineData(null, null, null, "car", null, null)]
        [InlineData(null, "business", null, null, 10, 2)]
        public async Task GetTopHeadlines_ValidQuery_ReturnsNews(string? country, string? category, string? sources, string? keyword, int? pageSize, int? page)
        {
            var fakeDTO = new TopHeadlinesSourceDTO { Country = country, Category = category, Sources = sources, Keyword = keyword, PageSize = pageSize, Page = page };

            var expectedResponse = new NewsApiResponseDTO { Status = "ok", TotalResult = 2, Articles = new List<ArticleDTO>() };

            _newsRepository.Setup(c => c.GetTopHeadlines(fakeDTO, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            var result = await _newsLogic.GetTopHeadlines(fakeDTO);

            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Status, result.Status);
            Assert.Equal(expectedResponse.TotalResult, result.TotalResult);
            Assert.Equal(expectedResponse.Articles.Count, result.Articles.Count);
        }

        [Theory]
        [InlineData(null, null, null, null, null, null)]
        [InlineData("us", "health", "fox-news", null, null, null)]
        [InlineData("us", null, "fox-news", null, null, null)]
        [InlineData(null, "business", "cbs-news", null, 10, 2)]
        public async Task GetTopHeadlines_InvalidQuery_ThrowsValidationException(string? country, string? category, string? sources, string? keyword, int? pageSize, int? page)
        {
            var fakeDTO = new TopHeadlinesSourceDTO { Country = country, Category = category, Sources = sources, Keyword = keyword, PageSize = pageSize, Page = page };

            await Assert.ThrowsAsync<ValidationException>(async () => await _newsLogic.GetTopHeadlines(fakeDTO));

            _newsRepository.Verify(c => c.GetTopHeadlines(fakeDTO, It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
