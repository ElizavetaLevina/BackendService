using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface INewsLogic
    {
        /// <summary>
        /// Получение новостей по параметрам
        /// </summary>
        /// <param name="topHeadlines">параметры</param>
        /// <param name="token">токен отмены</param>
        /// <returns>новости</returns>
        Task<NewsApiResponseDTO> GetTopHeadlines(TopHeadlinesSourceDTO topHeadlines, CancellationToken token = default);
    }
}
