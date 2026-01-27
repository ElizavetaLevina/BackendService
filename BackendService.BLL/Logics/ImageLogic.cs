using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BackendService.BLL.Logics
{
    public class ImageLogic(IImageRepository imageRepository) : IImageLogic
    {
        readonly IImageRepository _imageRepository = imageRepository;

        public async Task<List<ImageViewDTO>> GetPostImages(int postId, CancellationToken token = default)
        {
            if (postId <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            var images = await _imageRepository.GetPostImages(postId, token);

            return images is null ? throw new NotFoundException($"Пост с ID {postId} не найден") : images;
        }

        public async Task DeleteImage(int id, CancellationToken token = default)
        {
            if (id <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            try
            {
                await _imageRepository.DeleteImage(id, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Картинка с ID {id} не найдена и не может быть удалена");
            }
        }

        public async Task<int> SaveImage(IFormFile image, int postId, CancellationToken token = default)
        {
            if (image is null || image.Length == 0) throw new ValidationException("Файл не выбран");

            if (image.Length > 10 * 1024 * 1024) throw new ValidationException("Файл не должен превышать 10MB");

            if (!image.ContentType.StartsWith("image/")) throw new ValidationException("Загружаемый файл должен быть изображением");

            byte[] data; 

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream, token);
                data = memoryStream.ToArray();
            }

            try
            {
                return await _imageRepository.SaveImage(data, postId, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Пост с ID {postId} не найден");
            }
        }
    }
}
