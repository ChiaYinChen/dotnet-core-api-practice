using WebApiApp.Models;

namespace WebApiApp.Helpers
{
    public class ResponseHelper
    {
        public static Response<T> Success<T>(T data, Pagination? paging = null, string message = "Success", string code = "0000")
        {
            return new Response<T>
            {
                Code = code,
                Message = message,
                Data = data,
                Paging = paging
            };
        }
    }
}
