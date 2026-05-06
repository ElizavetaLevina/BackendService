using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendService.DAL.Models
{
    /// <summary>
    /// Пост
    /// </summary>
    public class PostEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название поста
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Текст поста
        /// </summary>
        public string TextPost { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания поста
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Дата последнего изменения поста
        /// </summary>
        public DateTime? DateUpdate { get; set; } = null;

        /// <summary>
        /// Удалён ли пост
        /// </summary>
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Автор поста
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Ссылка на версию поста на модерации
        /// </summary>
        public int? PostPendingId { get; set; } = null;

        /// <summary>
        /// Список тегов
        /// </summary>
        public virtual ICollection<TagEntity> Tags { get; set; } = [];

        /// <summary>
        /// Список картинок
        /// </summary>
        public virtual ICollection<ImageEntity>? Images { get; set; } = null;

        /// <summary>
        /// Версия поста на модерации
        /// </summary>
        [ForeignKey(nameof(PostPendingId))]
        public virtual PostPendingEntity? PendingVersion { get; set; } = null;
    }
}
