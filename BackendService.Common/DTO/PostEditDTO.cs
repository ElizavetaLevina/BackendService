namespace BackendService.Common.DTO
{
    /// <summary>
    /// Редактирование поста
    /// </summary>
    public class PostEditDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string TextPost { get; set; } = string.Empty;

        public HashSet<int> Tags { get; set; } = [];
        public HashSet<int> Images { get; set; } = [];
    }
}
