using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendService.DAL.Models
{
    /// <summary>
    /// Картинка
    /// </summary>
    public class ImageEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Картинка в виде массива байтов
        /// </summary>
        public byte[] Data { get; set; } = [];

        /// <summary>
        /// Удалена ли картинка
        /// </summary>
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Ссылка на пост
        /// </summary>
        public int PostId { get; set; }
		[ForeignKey("PostId")]
		[InverseProperty("Images")]

		/// Пост
		public virtual PostEntity Post { get; set; } = new();
    }
}
