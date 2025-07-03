using Microsoft.EntityFrameworkCore;
using WebApiApp.Models;

namespace WebApiApp.Helpers
{
    public class PaginationHelper
    {
        public static async Task<(List<T> Data, Pagination Paging)> ApplyPagination<T>(
            IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            // 計算總記錄數
            int totalRecords = await query.CountAsync();
            
            // 取得當前頁資料
            List<T> data = await query.Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();
            
            // 建立分頁資訊
            var pagingInfo = new Pagination(pageNumber, pageSize, totalRecords);

            // 返回資料和分頁資訊
            return (data, pagingInfo!);
        }
    }
}