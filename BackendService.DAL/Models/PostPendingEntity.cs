using Shared.Contracts.Enum;
using System.ComponentModel.DataAnnotations;

namespace BackendService.DAL.Models
{
    /// <summary>
    /// Пост на модерации
    /// </summary>
    public class PostPendingEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на PostEntity
        /// </summary>
        public int? PostId { get; set; }

        /// <summary>
        /// Название поста
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Текст поста
        /// </summary>
        public string TextPost { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания и отправки на модерацию
        /// </summary>
        public DateTime DateCreate { get; set; } = DateTime.Now;

        /// <summary>
        /// Автор поста
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Статус модерации
        /// </summary>
        public StatusModerationEnum Status { get; set; } = StatusModerationEnum.Pending;

        /// <summary>
        /// Причина отклонения
        /// </summary>
        public string? RejectionReason { get; set; } = null;

        /// <summary>
        /// Дата завершения модерации
        /// </summary>
        public DateTime? DateModerate { get; set; } = null;

        /// <summary>
        /// Множество идентификаторов тегов
        /// </summary>
        public IList<int> TagIds { get; set; } = [];

        /// <summary>
        /// Множество идентификаторов картинок
        /// </summary>
        public IList<int> ImageIds { get; set; } = [];
    }
}
