namespace BackendService.DAL.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string TextPost { get; set; } = string.Empty;

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        public HashSet<int> Tags { get; set; } = [];
    }
}
