namespace WebApiApp.Helpers
{
    public static class TimeHelper
    {
        private static readonly TimeZoneInfo AppTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");

        public static DateTime Now()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, AppTimeZone);
        }
    }
}
