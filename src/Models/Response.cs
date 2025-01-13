namespace WebApiApp.Models
{
    public class Response<T>
    {
        public required string Code { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }

    public class Response
    {
        public required string Code { get; set; }
        public required string Message { get; set; }
        public object? Errors { get; set; }
    }
}
