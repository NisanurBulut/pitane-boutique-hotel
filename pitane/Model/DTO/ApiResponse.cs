using System.Net;

namespace pitaneAPI.Model.DTO
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public TData? Data { get; set; }
        public object? Errors { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<TData> Create(bool success, int statusCode, string message = "", TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }
        public static ApiResponse<TData> NotFound(string message = "Resource not found")
        {
            return Create(false, (int)HttpStatusCode.NotFound, message);
        }
        public static ApiResponse<TData> BadRequest(string message = "Bad request", object? errors = null)
        {
            return Create(false, (int)HttpStatusCode.BadRequest, message, errors: errors);
        }
        public static ApiResponse<TData> Ok(TData data, string message = "Success")
        {
            return Create(true, (int)HttpStatusCode.OK, message, data);
        }
        public static ApiResponse<TData> InternalServerError(string message = "Internal server error", object? errors = null)
        {
            return Create(false, (int)HttpStatusCode.InternalServerError, message, errors: errors);
        }
        public static ApiResponse<TData> Conflict(string message = "Conflict", object? errors = null)
        {
            return Create(false, (int)HttpStatusCode.Conflict, message, errors: errors);
        }
        public static ApiResponse<TData> CreatedAt(string message = "Resource created", TData? data = default)
        {
            return Create(true, (int)HttpStatusCode.Created, message, data);
        }
        public static ApiResponse<TData> NoContent(string message = "No content")
        {
            return Create(true, (int)HttpStatusCode.NoContent, message);
        }
    }
}