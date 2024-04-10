using System;
using System.Collections.Generic;

public class Calendar
{
    public DateTime FirstDayOfMonth { get; }
    public List<DateTime> DaysOfMonth { get; }
    public int NumRows { get; }

    public Calendar(DateTime currentDate)
    {
        FirstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        DayOfWeek firstDayOfWeek = FirstDayOfMonth.DayOfWeek;
        int numberOfDaysInMonth = DateTime.DaysInMonth(FirstDayOfMonth.Year, FirstDayOfMonth.Month);

        NumRows = (int)Math.Ceiling((double)(numberOfDaysInMonth + (int)firstDayOfWeek) / 7);

        DaysOfMonth = new List<DateTime>();

        for (int i = 0; i < NumRows * 7; i++)
        {
            int dayOffset = i - (int)firstDayOfWeek + 1;
            DateTime date = FirstDayOfMonth.AddDays(dayOffset);
            if (dayOffset >= 0 && dayOffset < numberOfDaysInMonth)
            {
                DaysOfMonth.Add(date);
            }
            else
            {
                DaysOfMonth.Add(DateTime.MinValue);
            }
        }
    }
}
