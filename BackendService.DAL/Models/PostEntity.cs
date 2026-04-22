using System.ComponentModel.DataAnnotations;

namespace BackendService.DAL.Models
{
    public class PostEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string TextPost { get; set; } = string.Empty;

        public DateTime DateCreate { get; set; }

        public DateTime? DateUpdate { get; set; } = null;

        public bool Deleted { get; set; } = false;

        public Guid UserId { get; set; }

        public virtual ICollection<TagEntity> Tags { get; set; } = [];

        public virtual ICollection<ImageEntity>? Images { get; set; } = null;
    }
}
