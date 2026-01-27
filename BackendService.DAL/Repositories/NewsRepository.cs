using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using BackendService.Common.Helpers;
using System.Text.Json;

namespace BackendService.DAL.Repositories
{
    public class NewsRepository(string apiKey, string baseUrl) : INewsRepository
    {
        private readonly string _apiKey = apiKey;
        private readonly string _baseUrl = baseUrl;
        private readonly HttpClient _httpClient = new()
        {
            DefaultRequestHeaders = { { "User-Agent", "BackendService/1.0" } }
        };


        public async Task<NewsApiResponseDTO> GetTopHeadlines(TopHeadlinesSourceDTO topHeadlines, CancellationToken token = default)
        {
            var url = new QueryStringBuilderHelper($"{_baseUrl}/top-headlines?")
                .AddParam("country", topHeadlines.Country)
                .AddParam("category", topHeadlines.Category)
                .AddParam("sources", topHeadlines.Sources)
                .AddParam("q", topHeadlines.Keyword)
                .AddParam("pageSize", topHeadlines.PageSize?.ToString())
                .AddParam("page", topHeadlines.Page?.ToString())
                .AddApiKey(_apiKey)
                .Build();

            using var response = await _httpClient.GetAsync(url, token);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync(token);
                throw new HttpRequestException($"API error: {errorResponse}");
            }

            using var content = await response.Content.ReadAsStreamAsync(token);
            var result = await JsonSerializer.DeserializeAsync<NewsApiResponseDTO>(content, cancellationToken: token);

            return result ?? throw new InvalidOperationException("Failed to deserialize");
        }
    }
}
