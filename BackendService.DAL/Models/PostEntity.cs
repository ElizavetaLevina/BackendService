using System.ComponentModel.DataAnnotations;

namespace BackendService.DAL.Models
{
    public class PostEntity
    {
        public int Id { get; set; }

        public string TextPost { get; set; } = string.Empty;

        public DateTime DataCreate { get; set; }

        public DateTime DataUpdate { get; set; }

        public bool Deleted { get; set; } = false;

        public ICollection<TagEntity> Tags { get; set; } = [];
    }
}
