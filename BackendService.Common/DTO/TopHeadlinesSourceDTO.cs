using System.ComponentModel;

namespace BackendService.Common.DTO
{
    /// <summary>
    /// Параметры запроса для формирования url
    /// </summary>
    public class TopHeadlinesSourceDTO
    {
        public string? Country { get; set; } = null;

        public string? Category { get; set; } = null;

        public string? Sources { get; set; } = null;

        public string? Keyword { get; set; } = null;

        public int? PageSize { get; set; } = null;

        public int? Page { get; set; } = null;
    }
}
