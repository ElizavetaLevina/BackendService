using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController(INewsRepository newsService) : ControllerBase
    {
        private readonly INewsRepository _newsService = newsService;

        /// <summary>
        /// Получение новостей по параметрам
        /// </summary>
        /// <param name="topHeadlines">параметры</param>
        /// <param name="token">токен отмены</param>
        /// <returns>ответ от внешнего API</returns>
        [SwaggerOperation(Summary = "Получение новостей по параметрам", Description = "Получение новостей по указанным параметрам")]
        [SwaggerResponse(200, "Успешный ответ", typeof(NewsApiResponseDTO))]
        [SwaggerResponse(400, "Ошибка валидации")]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        [HttpGet("top-headlines")]
        public async Task<ActionResult<NewsApiResponseDTO>> GetTopHeadlines([FromQuery] TopHeadlinesSourceDTO topHeadlines, CancellationToken token = default)
        {
            return Ok(await _newsService.GetTopHeadlines(topHeadlines, token));
        }
    }
}
