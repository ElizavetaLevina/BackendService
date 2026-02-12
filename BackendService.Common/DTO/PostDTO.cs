namespace BackendService.Common.DTO
{
    /// <summary>
    /// Просмотр поста
    /// </summary>
    public class PostDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string TextPost { get; set; } = string.Empty;

        public DateTime DateCreate { get; set; }

        public DateTime? DateUpdate { get; set; } = null;

        public HashSet<int> Tags { get; set; } = [];

        public HashSet<int> Images { get; set; } = [];
    }
}
