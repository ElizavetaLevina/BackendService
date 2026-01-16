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
        /// <param name="id">идентификатор поста</param>
        /// <returns>пост</returns>
        Task<PostDTO?> GetPostById(int id, CancellationToken token = default);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <returns>задача удаления</returns>
        Task DeletePost(int id, CancellationToken token = default);

        /// <summary>
        /// Сохранение поста
        /// </summary>
        /// <param name="post">пост</param>
        /// <returns>сохранённый пост</returns>
        Task<PostEditDTO> SavePost(PostEditDTO post, CancellationToken token = default);
    }
}
