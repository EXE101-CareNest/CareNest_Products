namespace Shared.Helper
{
    public static class TimeHelper
    {
        public static DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetCurrentTimeVietnam()
        {
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
