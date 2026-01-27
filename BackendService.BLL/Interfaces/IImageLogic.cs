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
        public Task<List<ImageViewDTO>> GetPostImages(int postId, CancellationToken token = default);

        /// <summary>
        /// Удаление картинки
        /// </summary>
        /// <param name="id">id картинки</param>
        /// <param name="token">токен отмены</param>
        /// <returns>задача удаления</returns>
        public Task DeleteImage(int id, CancellationToken token = default);

        /// <summary>
        /// Загрузка картинки для поста
        /// </summary>
        /// <param name="image">картинка для загрузки</param>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>id сохранённой картинки</returns>
        public Task<int> SaveImage(IFormFile image, int postId, CancellationToken token = default);
    }
}
