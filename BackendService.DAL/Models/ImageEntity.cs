using System.ComponentModel.DataAnnotations.Schema;

namespace BackendService.DAL.Models
{
    public class ImageEntity
    {       
        public int Id { get; set; }
        public byte[] Data { get; set; } = [];
        public bool Deleted { get; set; } = false;
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        [InverseProperty("Images")]
        public virtual PostEntity Post { get; set; }
    }
}
