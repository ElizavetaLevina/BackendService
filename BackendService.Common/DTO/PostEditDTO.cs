namespace BackendService.Common.DTO
{
    public class PostEditDTO
    {
        public int Id { get; set; }

        public string TextPost { get; set; } = string.Empty;

        public HashSet<int> Tags { get; set; } = [];
    }
}
