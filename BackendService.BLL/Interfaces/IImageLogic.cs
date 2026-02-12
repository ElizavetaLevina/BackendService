using BackendService.Common.DTO;
using Microsoft.AspNetCore.Http;

namespace BackendService.BLL.Interfaces
{
    public interface IImageLogic
    {
        /// <summary>
        /// Получение списка картинок по id поста
        /// </summary>
        /// <param name="postId">id поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>список картинок</returns>
        Task<List<ImageViewDTO>> GetPostImages(int postId, CancellationToken token = default);

        /// <summary>
        /// Удаление картинки
        /// </summary>
        /// <param name="imageId">id картинки</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="token">токен отмены</param>
        /// <returns>задача удаления</returns>
        Task DeleteImage(int imageId, Guid userId, CancellationToken token = default);

        /// <summary>
        /// Загрузка картинки для поста
        /// </summary>
        /// <param name="image">картинка для загрузки</param>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="token">токен отмены</param>
        /// <returns>id сохранённой картинки</returns>
        Task<int> SaveImage(IFormFile image, int postId, Guid userId, CancellationToken token = default);

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
