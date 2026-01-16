using System.Net;

namespace BackendService.Common.Exceptions
{
    public class ValidationException(string message) : BaseException(message, HttpStatusCode.BadRequest)
    {
    }
}
