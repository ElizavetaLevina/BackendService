using System.Text.Json.Serialization;

namespace BackendService.Common.DTO
{
    /// <summary>
    /// Источник новости
    /// </summary>
    public class NewsSourceDTO
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
