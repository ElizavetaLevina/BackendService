namespace BackendService.DAL.DTO
{
    public class TagEditDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<PostEditDTO> Posts { get; set; } = [];
    }
}
