using System.Net;

namespace PATK.Domain.Rest
{
    public class ApiResponse<T>
    {
        public ApiResponse(HttpStatusCode statusCode, string message, T content, ApiError error = null)
        {
            StatusCode = statusCode;
            Message = message;
            Content = content;
            Error = error;
        }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }
        public ApiError Error { get; set; }
    }
}