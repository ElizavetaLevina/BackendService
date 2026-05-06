namespace BackendService.Common.DTO
{
    public class PostPendingEditDTO
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на PostEntity
        /// </summary>
        public int? PostId { get; set; } = null;

        /// <summary>
        /// Название поста
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Текст поста
        /// </summary>
        public string TextPost { get; set; } = string.Empty;

        /// <summary>
        /// Автор поста
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Множество идентификаторов тегов
        /// </summary>
        public HashSet<int> TagIds { get; set; } = [];

        /// <summary>
        /// Множество идентификаторов картинок
        /// </summary>
        public HashSet<int> ImageIds { get; set; } = [];
    }
}
