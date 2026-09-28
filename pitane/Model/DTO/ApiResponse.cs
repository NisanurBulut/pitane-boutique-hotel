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
    }
}
