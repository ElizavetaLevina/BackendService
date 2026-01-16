using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostLogic postLogic) : ControllerBase
    {
        private readonly IPostLogic _postLogic = postLogic;

        /// <summary>
        /// Получение коллекции постов
        /// </summary>
        /// <param name="token">токен отмены</param>
        /// <returns>коллекция постов</returns>
        [SwaggerOperation(Summary = "Получение коллекции постов", Description = "Возвращает коллекцию всех постов из базы данных")]
        [SwaggerResponse(200, "Успешный ответ", typeof(ICollection<PostDTO>))]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpGet("list")]
        public async Task<ActionResult<ICollection<PostDTO>>> GetPosts(CancellationToken token = default)
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
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Пост с указанным ID не найден")]
        [SwaggerResponse(204, "Пост успешно удалён")]
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
    }
}
