using System.ComponentModel.DataAnnotations;

namespace BackendService.DAL.Models
{
    /// <summary>
    /// Тег
    /// </summary>
    public class TagEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название тега
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания тега
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Удалён ли тег
        /// </summary>
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Список постов
        /// </summary>
        public virtual ICollection<PostEntity> Posts { get; set; } = [];
    }
}
