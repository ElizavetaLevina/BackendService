using System.Net;

namespace BackendService.Common.Exceptions
{
    public class NotFoundException(string message) : BaseException(message, HttpStatusCode.NotFound)
    {
    }
}
