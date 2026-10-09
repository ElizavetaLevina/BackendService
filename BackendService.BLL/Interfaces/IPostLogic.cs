using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface IPostLogic
    {
        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <param name="page">Номер страницы (начиная с 1)</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token"Токен отмены</param>
        /// <returns>Список постов</returns>
        Task<List<PostDTO>> GetPosts(int page, int pageSize, CancellationToken token = default);

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="token"Токен отмены</param>

        /// <returns>пост</returns>
        Task<PostDTO?> GetPostById(int postId, CancellationToken token = default);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="token"Токен отмены</param>
        /// <returns>задача удаления</returns>
        Task DeletePost(int postId, Guid userId, CancellationToken token = default);
    }
}
