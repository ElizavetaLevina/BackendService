using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface IPostLogic
    {
        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <returns>список постов</returns>
        Task<List<PostDTO>> GetPosts(CancellationToken token = default);

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="postId">идентификатор поста</param>
        /// <returns>пост</returns>
        Task<PostDTO?> GetPostById(int postId, CancellationToken token = default);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="postId">идентификатор поста</param>
        /// <returns>задача удаления</returns>
        Task DeletePost(int postId, Guid userId, CancellationToken token = default);

        /// <summary>
        /// Сохранение поста
        /// </summary>
        /// <param name="post">пост</param>
        /// <returns>сохранённый пост</returns>
        Task<PostEditDTO> SavePost(PostEditDTO post, Guid userId, CancellationToken token = default);

        /// <summary>
        /// Проверяет, является ли указанный пользователь владельцем поста
        /// </summary>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="userId">идентификатор пользователя для проверки</param>
        /// <param name="token">токен отмены</param>
        /// <returns>результат проверки</returns>
        Task<bool> IsPostOwner(int postId, Guid userId, CancellationToken token = default);
    }
}
