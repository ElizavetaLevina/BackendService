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
        public Task<List<PostDTO>> GetPosts(CancellationToken token = default);

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>пост</returns>
        public Task<PostDTO?> GetPostById(int id, CancellationToken token = default);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>задача удаления</returns>
        public Task DeletePost(int id, CancellationToken token = default);


        /// <summary>
        /// Сохранение поста
        /// </summary>
        /// <param name="postEntity">пост для сохранения</param>
        /// <param name="token">токен отмены</param>
        /// <returns>сохранённый пост</returns>
        public Task<PostEditDTO> SavePost(PostEditDTO postEntity, CancellationToken token = default);
    }
}
