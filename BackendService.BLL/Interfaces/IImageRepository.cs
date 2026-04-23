using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface IImageRepository
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
        /// <param name="token">токен отмены</param>
        /// <returns>задача удаления</returns>
        Task DeleteImage(int imageId, CancellationToken token = default);

        /// <summary>
        /// Сохранение картинки
        /// </summary>
        /// <param name="data">картинка в виде массива байтов</param>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>id сохранённой картинки</returns>
        Task<int> SaveImage(byte[] data, int postId, CancellationToken token = default);

        /// <summary>
        /// Возвращает идентификатор поста, которому принадлежит картинка
        /// </summary>
        /// <param name="imageId">идентификатор картинки</param>
        /// <param name="token">токен отмены</param>
        /// <returns>идентификатор поста</returns>
        Task<int?> GetPostIdByImageId(int imageId, CancellationToken token = default);
    }
}
