using BackendService.DAL.Models;

namespace BackendService.DAL.Interfaces
{
    public interface IPostRepository
    {
        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <param name="token">токен отмены</param>
        /// <returns>список постов</returns>
        public Task<List<PostEntity>> GetPosts(CancellationToken token = default);

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>пост</returns>
        public Task<PostEntity?> GetPostById(int id, CancellationToken token = default);

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
        public Task<PostEntity> SavePost(PostEntity postEntity, CancellationToken token = default);
    }
}
