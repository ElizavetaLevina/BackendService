using System.Text.Json.Serialization;

namespace BackendService.Common.DTO
{
    /// <summary>
    /// Ответ от NewsApi
    /// </summary>
    public class NewsApiResponseDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("totalResults")]
        public int TotalResult { get; set; }

        [JsonPropertyName("articles")]
        public List<ArticleDTO> Articles { get; set;  } = [];
    }
}
