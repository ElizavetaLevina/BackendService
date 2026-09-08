using BackendService.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendService.API.Controllers
{
    /// <summary>
    /// Базовый контроллер с утилитами для получения данных пользователя и нормализации токена отмены
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        protected Guid GetUserIdGuid()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                throw new ForbiddenException("User ID не найден в claims");

            return Guid.Parse(userId);
        }

        protected CancellationToken EffectiveCancellationToken(CancellationToken token)
        {
            return token == default ? HttpContext.RequestAborted : token;
        }
    }
}
