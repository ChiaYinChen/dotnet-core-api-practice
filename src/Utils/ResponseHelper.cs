using WebApiApp.Models;

namespace WebApiApp.Helpers
{
    public class ResponseHelper
    {
        public static Response<T> Success<T>(T data, string message = "Success", string code = "0000")
        {
            return new Response<T>
            {
                Code = code,
                Message = message,
                Data = data
            };
        }

        public static Response Error(string message, object? errors = null, string code = "9999")
        {
            return new Response
            {
                Code = code,
                Message = message,
                Errors = errors
            };
        }
    }
}
