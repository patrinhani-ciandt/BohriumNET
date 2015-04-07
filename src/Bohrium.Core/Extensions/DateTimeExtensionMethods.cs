namespace Bohrium.Core.Extensions
{
    using System;

    public static class DateTimeExtensionMethods
    {
        /// <summary>
        /// Returns the first day of the month for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The first day of the month for the specified date</returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns the first week day of the month for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The first week day of the month for the specified date</returns>
        public static DateTime FirstWeekDayOfMonth(this DateTime date)
        {
            var dateTime = date.FirstDayOfMonth();

            while (!dateTime.IsWeekDay())
            {
                dateTime = dateTime.AddDays(1);
            }

            return dateTime;
        }

        /// <summary>
        /// Returns the next week day of the month for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The next week day of the month for the specified date</returns>
        public static DateTime NextWeekDayOfMonth(this DateTime date)
        {
            var dateTime = date;

            while (!dateTime.IsWeekDay())
            {
                dateTime = dateTime.AddDays(1);
            }

            return dateTime;
        }

        /// <summary>
        /// Returns the previous week day of the month for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The previous week day of the month for the specified date</returns>
        public static DateTime PreviousWeekDayOfMonth(this DateTime date)
        {
            var dateTime = date;

            while (!dateTime.IsWeekDay())
            {
                dateTime = dateTime.AddDays(-1);
            }

            return dateTime;
        }

        /// <summary>
        /// Returns the last day of the month for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The last day of the month for the specified date</returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return FirstDayOfMonth(date).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Returns the last week day of the month for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The last week day of the month for the specified date</returns>
        public static DateTime LastWeekDayOfMonth(this DateTime date)
        {
            var dateTime = date.LastDayOfMonth();

            while (!dateTime.IsWeekDay())
            {
                dateTime = dateTime.AddDays(-1);
            }

            return dateTime;
        }

        /// <summary>
        /// Returns the first day of the week for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The first day of the week for the specified date</returns>
        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            return date.Date.AddDays(-(int)date.DayOfWeek);
        }

        /// <summary>
        /// Returns the last day of the week for the specified date
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The last day of the week for the specified date</returns>
        public static DateTime LastDayOfWeek(this DateTime date)
        {
            return FirstDayOfWeek(date).AddDays(6);
        }

        /// <summary>
        /// Returns the date at 23:59.59.999 for the specified DateTime
        /// </summary>
        /// <param name="date">The DateTime to be processed</param>
        /// <returns>The date at 23:50.59.999</returns>
        public static DateTime GetEndOfDay(this DateTime date)
        {
            return date.SetTime(23, 59, 59, 999);
        }

        /// <summary>
        /// Returns a new DateTime instance with the time specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="miliseconds"></param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime date, int hours = 0, int minutes = 0, int seconds = 0, int miliseconds = 0)
        {
            return date.Date
                .AddHours(hours)
                .AddMinutes(minutes)
                .AddSeconds(seconds)
                .AddMilliseconds(miliseconds);
        }

        /// <summary>
        /// Returns whether the DateTime falls on a weekend day
        /// </summary>
        /// <param name="date">The date to be processed</param>
        /// <returns>Whether the specified date occurs on a weekend</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns whether the DateTime falls on a weekday
        /// </summary>
        /// <param name="date">The date to be processed</param>
        /// <returns>Whether the specified date occurs on a weekday</returns>
        public static bool IsWeekDay(this DateTime date)
        {
            return !date.IsWeekend();
        }
    }
}