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
        Task SavePost(PostEditDTO post, Guid userId, CancellationToken token = default);
    }
}
