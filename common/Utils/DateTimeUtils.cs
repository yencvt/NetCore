using System;
using System.Globalization;

namespace common.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime ConvertStringToDateTime(string dateString, string format = null)
        {
            if (string.IsNullOrEmpty(format))
            {
                if (DateTime.TryParse(dateString, out DateTime result))
                {
                    return result;
                }
            }
            else
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
            throw new ArgumentException("Invalid date string format.");
        }

        public static long ConvertStringToLong(string dateString, string format = null)
        {
            DateTime dateTime = ConvertStringToDateTime(dateString, format);
            return dateTime.Ticks;
        }

        public static DateTime ConvertLongToDateTime(long ticks)
        {
            return new DateTime(ticks);
        }

        public static string ConvertDateTimeToString(DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        public static string ConvertLongToString(long ticks, string format)
        {
            DateTime dateTime = new DateTime(ticks);
            return dateTime.ToString(format);
        }

        public static int CompareDateTimes(DateTime dt1, DateTime dt2)
        {
            return DateTime.Compare(dt1, dt2);
        }

        public static int CompareLongs(long ticks1, long ticks2)
        {
            DateTime dt1 = new DateTime(ticks1);
            DateTime dt2 = new DateTime(ticks2);
            return DateTime.Compare(dt1, dt2);
        }
    }
}



