using System.ComponentModel.DataAnnotations;

namespace BackendService.DAL.Models
{
    public class TagEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool Deleted { get; set; } = false;

        public ICollection<PostEntity> Posts { get; set; } = [];
    }
}
