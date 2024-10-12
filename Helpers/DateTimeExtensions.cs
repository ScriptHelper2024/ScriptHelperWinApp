using System;
namespace Helpers
{
    public static class DateTimeExtensions
    {
        public static long AsEpoch(this DateTime utcDateTime)
        {
            return (long)(utcDateTime - epoch).TotalSeconds;
        }

        public static DateTime AsDateTime(this long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string AsMidLengthString(this DateTime dt)
        {
            int day = dt.Day;

            var suffix = (day % 10 == 1 && day != 11) ? "st"
                : (day % 10 == 2 && day != 12) ? "nd"
                : (day % 10 == 3 && day != 13) ? "rd"
                : "th";

            return dt.ToString("dddd, MMMM d") + suffix;
        }

        public static string AsNiceTime(this TimeSpan ts)
        {
            if (ts.TotalMinutes <= 70.0)
            {
                return $"{(int)ts.TotalMinutes} minute{Plural(ts.TotalMinutes)}" ;
            }
            if (ts.TotalDays > 1.0)
            {
                return $"{(int)ts.TotalDays} day{Plural(ts.TotalDays)}, {ts.Hours} hour{Plural(ts.Hours)}, {ts.Minutes} minute{Plural(ts.Minutes)}";
            }
                
            return $"{ts.Hours} hour{Plural(ts.Hours)}, {ts.Minutes} minute{Plural(ts.Minutes)}";
        }

        private static string Plural(int val)
        {
            return val == 1 ? "" : "s";
        }
        private static string Plural(double val)
        {
            return Plural((int)val);
        }

        public static DateTime WithTime(this DateTime date, TimeSpan time)
        {
            var result = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
            DateTime.SpecifyKind(result, date.Kind);
            return result;

        }

        public static bool IsDaysOlder(this DateTime date, double days)
        {
            return (DateTime.Now - date).TotalDays > days;
        }

        public static string ToElapsedTime(this TimeSpan ts)
        {
            return ts.ToString(@"hh\:mm\:ss");
        }

        public static string ISODate(this DateTime ts)
        {
            return ts.ToString(@"yyyy-MM-dd");
        }

        public static string ISODateTime(this DateTime ts)
        {
            return ts.ToString(@"yyyy-MM-ddThh:mm:ss");
        }

        public static int DaysInYear(this int year)
        {
            return DateTime.IsLeapYear(year) ? 366 : 365;
            
        }
    }
}
