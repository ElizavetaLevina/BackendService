using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;

namespace BackendService.BLL.Logics
{
    public class NewsLogic(INewsRepository newsRepository) : INewsLogic
    {
        private readonly INewsRepository _newsRepository = newsRepository;

        public async Task<NewsApiResponseDTO> GetTopHeadlines(TopHeadlinesSourceDTO topHeadlines, CancellationToken token = default)
        {
            if (topHeadlines.Country is null && topHeadlines.Category is null && topHeadlines.Sources is null && topHeadlines.Keyword is null) throw new ValidationException("Не указано ни одного параметра");
            if (topHeadlines.Country is not null && topHeadlines.Category is not null && topHeadlines.Sources is not null) throw new ValidationException("Параметр sources нельзя использовать совместно с параметром country и category");
            if (topHeadlines.Country is not null && topHeadlines.Sources is not null) throw new ValidationException("Параметр country нельзя использовать совместно с параметром sources");
            if (topHeadlines.Category is not null && topHeadlines.Sources is not null) throw new ValidationException("Параметр category нельзя использовать совместно с параметром sources");

            return await _newsRepository.GetTopHeadlines(topHeadlines, token);
        }
    }
}
