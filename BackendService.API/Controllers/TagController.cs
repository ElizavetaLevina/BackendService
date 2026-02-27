using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController(ITagLogic tagLogic) : ControllerBase
    {
        private readonly ITagLogic _tagLogic = tagLogic;

        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <param name="token">токен отмены</param>
        /// <returns>список тегов</returns>
        [SwaggerOperation(Summary = "Получение списка тегов", Description = "Возвращает список всех тегов из базы данных")]
        [SwaggerResponse(200, "Успешный ответ", typeof(IReadOnlyList<TagEditDTO>))]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [Authorize(Policy = "UserRead")]
        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<TagEditDTO>>> GetTags(CancellationToken token = default)
        {
            return Ok(await _tagLogic.GetTags(token));
        }

        /// <summary>
        /// Получение тега по идентификатору
        /// </summary>
        /// <param name="tagId">идентификатор тега</param>
        /// <param name="token">токен отмены</param>
        /// <returns>тег</returns>
        [SwaggerOperation(Summary = "Получение тега", Description = "Возвращает тег по его идентификатору из базы данных")]
        [SwaggerResponse(200, "Успешный ответ", typeof(TagEditDTO))]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Тег с указанным ID не найден")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [Authorize(Policy = "UserRead")]
        [HttpGet("{tagId}")]
        public async Task<ActionResult<TagEditDTO>> GetTagById(int tagId, CancellationToken token = default)
        {
            return Ok(await _tagLogic.GetTagById(tagId, token));
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="tagIid">идентификатор тега</param>
        /// <param name="token">токен отмены</param>
        /// <returns>пустой ответ со статусом 204 в случае успеха</returns>
        [SwaggerOperation(Summary = "Удаление тега", Description = "Удаляет тег по его идентификатору из базы данных")]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Тег с указанным ID не найден")]
        [SwaggerResponse(204, "Тег успешно удалён")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [Authorize(Policy = "Admin")]
        [HttpDelete("{tagId}")]
        public async Task<ActionResult> DeleteTag(int tagIid, CancellationToken token = default)
        {
            await _tagLogic.DeleteTag(tagIid, token);
            return NoContent();
        }

        /// <summary>
        /// Создание тега
        /// </summary>
        /// <param name="tag">тег</param>
        /// <param name="token">токен отмены</param>
        /// <returns>созданный тег</returns>
        [SwaggerOperation(Summary = "Создание тега", Description = "Создаёт тег")]
        [SwaggerResponse(200, "Тег успешно создан", typeof(PostDTO))]
        [SwaggerResponse(400, "Неверные данные тега")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<TagEditDTO>> CreateTag(TagEditDTO tag, CancellationToken token = default)
        {
            return Ok(await _tagLogic.SaveTag(tag, token));
        }

        /// <summary>
        /// Обновление тега
        /// </summary>
        /// <param name="tag">тег</param>
        /// <param name="token">токен отмены</param>
        /// <returns>обновлённый тег</returns>
        [SwaggerOperation(Summary = "Редактирование тега", Description = "Редактирует тег по его идентификатору в базе данных")]
        [SwaggerResponse(400, "Неверный формат ID (отрицательное или нулевое значение)")]
        [SwaggerResponse(404, "Тег с указанным ID не найден")]
        [SwaggerResponse(200, "Успешный ответ", typeof(PostDTO))]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<ActionResult<TagEditDTO>> UpdateTag(TagEditDTO tag, CancellationToken token = default)
        {
            return Ok(await _tagLogic.SaveTag(tag, token));
        }
    }
}