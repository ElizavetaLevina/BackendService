using BackendService.DAL.Models;

namespace BackendService.DAL.Interfaces
{
    public interface IPostLogic
    {
        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <returns>список постов</returns>
        Task<List<PostEntity>> GetPosts();

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <returns>пост</returns>
        Task<PostEntity?> GetPostById(int id);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <returns>задача удаления</returns>
        Task DeletePost(int id);

        /// <summary>
        /// Сохранение поста
        /// </summary>
        /// <param name="post">пост</param>
        /// <returns>сохранённый пост</returns>
        Task<PostEntity> SavePost(PostEntity post);       
    }
}
