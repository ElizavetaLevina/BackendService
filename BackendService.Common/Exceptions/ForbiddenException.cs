using System.Net;

namespace BackendService.Common.Exceptions
{
    public class ForbiddenException(string message) : BaseException(message, HttpStatusCode.Forbidden)
    {
    }
}
