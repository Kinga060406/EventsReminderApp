using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsReminderApp.MVC.Models
{
    public class Calendar
    {
        public DateTime CurrentDate { get; private set; }
        public List<DateTime> DaysOfMonth { get; private set; }

        public Calendar(DateTime currentDate)
        {
            CurrentDate = currentDate;
            DaysOfMonth = GenerateDaysOfMonth(currentDate);
        }

        private List<DateTime> GenerateDaysOfMonth(DateTime currentDate)
        {
            var days = new List<DateTime>();
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Add leading days from previous month
            var leadingDays = (int)firstDayOfMonth.DayOfWeek;
            for (int i = 0; i < leadingDays; i++)
            {
                days.Add(DateTime.MinValue); // Placeholder for empty days
            }

            // Add days of the current month
            for (var day = firstDayOfMonth; day <= lastDayOfMonth; day = day.AddDays(1))
            {
                days.Add(day);
            }

            // Add trailing days to fill the last week
            var trailingDays = 7 - days.Count % 7;
            if (trailingDays < 7)
            {
                for (int i = 0; i < trailingDays; i++)
                {
                    days.Add(DateTime.MinValue); // Placeholder for empty days
                }
            }

            return days;
        }
    }
}
