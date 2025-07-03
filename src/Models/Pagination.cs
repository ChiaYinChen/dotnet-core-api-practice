namespace WebApiApp.Models
{
    public record Pagination(
        int PageNumber,             // 當前頁碼
        int PageSize,               // 每頁顯示的資料數量
        int TotalRecords            // 資料庫中的總記錄數
    )
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize); // 計算總頁數
    }
}