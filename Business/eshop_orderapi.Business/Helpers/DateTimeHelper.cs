using System;
using System.Globalization;

namespace eshop_orderapi.Business.Helpers
{
    public static class DateTimeHelper
    {
        public static string DateFormat = Constant.DateFormat;
        public static string DateTimeFormat = Constant.DateTimeFormat;
        public static int TimeDifference = 0;

        public static string FormatDateToString(DateTime? datetime, bool addTimeDifference)
        {
            return GetDateTimeString(datetime, DateFormat, addTimeDifference);
        }

        public static string FormatDateTimeToString(DateTime? datetime, bool addTimeDifference)
        {
            return GetDateTimeString(datetime, DateTimeFormat, addTimeDifference);
        }

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        private static string GetDateTimeString(DateTime? datetime, string dateFormat, bool addTimeDifference)
        {
            if (datetime == null)
            {
                return string.Empty;
            }

            if (addTimeDifference)
            {
                datetime = datetime.Value.AddMinutes(TimeDifference);
            }

            var dateTime = datetime.Value.ToString(dateFormat);
            if (!DateTimeFormat.Contains('t'))
            {
                dateTime = datetime.Value.ToString(dateFormat.Replace('h', 'H'));
            }

            if (!DateFormat.Contains("-") && DateFormat.Contains("/"))
            {
                dateTime = dateTime.Replace("-", "/");
            }

            return dateTime;
        }

        public static DateTime? ToDateTime(this string date)
        {
            DateTime dateTime;
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }
            if (DateTime.TryParseExact(date,
                new[] { DateFormat, DateTimeFormat },
                CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            if (!DateTimeFormat.Contains('t') && DateTime.TryParseExact(date.Replace('-', '/').Replace('.', '/'), DateTimeFormat.Replace('h', 'H').Replace('-', '/').Replace('.', '/'), null, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            return null;
        }

        //Receives argument as HH:mm
        public static int ToMinutes(this string str)
        {
            DateTime start = Convert.ToDateTime(str);
            return start.Hour * 60 + start.Minute;
        }

        public static string ToFormattedHours(this int minutes)
        {
            return string.Format("{0:00}:{1:00}", (int)TimeSpan.FromMinutes(minutes).TotalHours, TimeSpan.FromMinutes(minutes).Minutes);
        }

        public static bool ToDateTime(this string fromDate, string toDate, string fromTime, string toTime, bool IsAllowMatchTime)
        {
            DateTime? fromdateTime;
            DateTime? todateTime;
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                return false;
            }
            if (string.IsNullOrEmpty(fromTime) || string.IsNullOrEmpty(toTime))
            {
                fromTime = "00:00:00";
                toTime = "00:00:00";
            }

            if (IsAllowMatchTime)
            {
                fromdateTime = new DateTime(ToDateTime(fromDate).Value.Year, ToDateTime(fromDate).Value.Month, ToDateTime(fromDate).Value.Day, Convert.ToInt32(fromTime.Split(':')[0]), Convert.ToInt32(fromTime.Split(':')[1]), 00);
                todateTime = new DateTime(ToDateTime(toDate).Value.Year, ToDateTime(toDate).Value.Month, ToDateTime(toDate).Value.Day, Convert.ToInt32(toTime.Split(':')[0]), Convert.ToInt32(toTime.Split(':')[1]), 00);

                if (todateTime < fromdateTime)
                {
                    return false;
                }
            }
            else
            {
                fromdateTime = new DateTime(ToDateTime(fromDate).Value.Year, ToDateTime(fromDate).Value.Month, ToDateTime(fromDate).Value.Day, Convert.ToInt32(fromTime.Split(':')[0]), Convert.ToInt32(fromTime.Split(':')[1]), 00);
                todateTime = new DateTime(ToDateTime(toDate).Value.Year, ToDateTime(toDate).Value.Month, ToDateTime(toDate).Value.Day, Convert.ToInt32(toTime.Split(':')[0]), Convert.ToInt32(toTime.Split(':')[1]), 00);
                if (todateTime <= fromdateTime)
                {
                    return false;
                }
            }
            return true;
        }
    }
}