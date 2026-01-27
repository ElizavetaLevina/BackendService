using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostLogic postLogic, IImageLogic imageLogic) : ControllerBase
    {
        private readonly IPostLogic _postLogic = postLogic;
        private readonly IImageLogic _imageLogic = imageLogic;

        /// <summary>
        /// Получение списка постов
        /// </summary>
        /// <param name="token">токен отмены</param>
        /// <returns>список постов</returns>
        [SwaggerOperation(Summary = "Получение списка постов", Description = "Возвращает список всех постов из базы данных")]
        [SwaggerResponse(200, "Успешный ответ", typeof(IReadOnlyList<PostDTO>))]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<PostDTO>>> GetPosts(CancellationToken token = default)
        {
            return Ok(await _postLogic.GetPosts(token));
        }

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>пост</returns>
        [SwaggerOperation(Summary = "Получение поста", Description = "Возвращает пост по его идентификатору из базы данных")]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Пост с указанным ID не найден")]
        [SwaggerResponse(200, "Успешный ответ", typeof(PostDTO))]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPostById(int id, CancellationToken token = default)
        {
            return Ok(await _postLogic.GetPostById(id, token));
        }

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="id">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>пустой ответ со статусом 204 в случае успеха</returns>
        [SwaggerOperation(Summary = "Удаление поста", Description = "Удаляет пост по его идентификатору из базы данных")]
        [SwaggerResponse(204, "Пост успешно удалён")]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Пост с указанным ID не найден")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id, CancellationToken token = default)
        {
            await _postLogic.DeletePost(id, token);
            return NoContent();
        }

        /// <summary>
        /// Создание поста
        /// </summary>
        /// <param name="post">пост</param>
        /// <param name="token">токен отмены</param>
        /// <returns>созданный пост</returns>
        [SwaggerOperation(Summary = "Создание поста", Description = "Создаёт пост")]
        [SwaggerResponse(200, "Пост успешно создан", typeof(PostEditDTO))]
        [SwaggerResponse(400, "Неверные данные поста")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpPost]
        public async Task<ActionResult<PostEditDTO>> CreatePost(PostEditDTO post, CancellationToken token = default)
        {
            return Ok(await _postLogic.SavePost(post, token));
        }

        /// <summary>
        /// Обновление поста
        /// </summary>
        /// <param name="post">пост</param>
        /// <param name="token">токен отмены</param>
        /// <returns>обновлённый пост</returns>
        [SwaggerOperation(Summary = "Редактирование поста", Description = "Редактирует пост по его идентификатору в базе данных")]
        [SwaggerResponse(200, "Успешный ответ", typeof(PostEditDTO))]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Пост с указанным ID не найден")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpPut]
        public async Task<ActionResult<PostEditDTO>> UpdatePost(PostEditDTO post, CancellationToken token = default)
        {
            return Ok(await _postLogic.SavePost(post, token));
        }

        /// <summary>
        /// Получение списка картинок для поста
        /// </summary>
        /// <param name="postId">id поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>список с данными картинок</returns>
        [SwaggerOperation(Summary = "Получение списка картинок для поста", Description = "Возвращает список картинок для поста по его идентификатору из базы данных")]
        [SwaggerResponse(200, "Успешный ответ")]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Пост с указанным ID не найден")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpGet("images_list")]
        public async Task<ActionResult> GetPostImages(int postId, CancellationToken token = default)
        {
            var images = await _imageLogic.GetPostImages(postId, token);
            
            return Ok(images);
        }

        /// <summary>
        /// Удаление картинки
        /// </summary>
        /// <param name="id">идентификатор картинки</param>
        /// <param name="token">токен отмены</param>
        /// <returns>пустой ответ со статусом 204 в случае успеха</returns>
        [SwaggerOperation(Summary = "Удаление картинки", Description = "Удаляет картинку по её идентификатору из базы данных")]
        [SwaggerResponse(204, "Картинка успешно удалена")]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Картинка с указанным ID не найдена")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpDelete("image_{id}")]
        public async Task<ActionResult> DeleteImage(int id, CancellationToken token = default)
        {
            await _imageLogic.DeleteImage(id, token);
            return NoContent();
        }

        /// <summary>
        /// Загрузка картинки
        /// </summary>
        /// <param name="image">картинка для загрузки</param>
        /// <param name="postId">идентификатор поста</param>
        /// <param name="token">токен отмены</param>
        /// <returns>загруженная картинка</returns>
        [SwaggerOperation(Summary = "Загрузка картинки", Description = "Загружает картинку для поста в базу данных")]
        [SwaggerResponse(200, "Картинка успешно загружена")]
        [SwaggerResponse(400, "Неверные данные")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpPost("image")]
        public async Task<ActionResult<int>> CreateImage(IFormFile image, int postId, CancellationToken token = default)
        {
            var result = await _imageLogic.SaveImage(image, postId, token);
            return Ok(result);
        }
    }
}
