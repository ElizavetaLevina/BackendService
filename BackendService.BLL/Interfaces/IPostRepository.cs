using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface IPostRepository
    {
        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <param name="token">токен отмены</param>
        /// <returns>список постов</returns>
        Task<List<PostDTO>> GetPosts(CancellationToken token = default);

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>пост</returns>
        Task<PostDTO?> GetPostById(int postId, CancellationToken token = default);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>удален ли пост</returns>
        Task<bool> DeletePost(int postId, CancellationToken token = default);

        /// <summary>
        /// Сохранение поста
        /// </summary>
        /// <param name="post">пост для сохранения</param>
        /// <param name="token">токен отмены</param>
		/// <returns>сохранен ли пост</returns>
        Task<bool> SavePost(PostEditDTO post, Guid userId, CancellationToken token = default);

		/// <summary>
		/// Возвращает идентификатор владельца поста
		/// </summary>
		/// <param name="postId">идентификатор поста</param>
		/// <param name="token">токен отмены</param>
		/// <returns>идентификатор владельца поста</returns>
		Task<Guid?> GetUserIdByPostId(int postId, CancellationToken token = default);
    }
}
