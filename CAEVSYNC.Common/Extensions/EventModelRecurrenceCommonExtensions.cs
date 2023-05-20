using CAEVSYNC.Common.Models;
using Ical.Net;
using Ical.Net.DataTypes;

namespace CAEVSYNC.Common.Extentions;

public static class EventModelRecurrenceCommonExtensions
{
    public static string ToICalRecurrencePattern(this MicrosoftRecurrence microsoftRecurrence)
    {
        if (microsoftRecurrence == null || !microsoftRecurrence.IsValid())
            return null;
        
        var recurrencePattern = new RecurrencePattern(microsoftRecurrence.Pattern.Type.ToICalFrequencyType());

        recurrencePattern.Interval = microsoftRecurrence.Pattern.Interval;

        if (microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.WEEKLY)
        {
            recurrencePattern.FirstDayOfWeek = microsoftRecurrence.Pattern.FirstDayOfWeek.ToICalDayOfWeek();
            recurrencePattern.ByDay = microsoftRecurrence.Pattern.DaysOfWeek
                .Select(d => new WeekDay { DayOfWeek = d.ToICalDayOfWeek() })
                .ToList();
        }

        // if (microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.ABSOLUTE_MONTHLY)
        // {
        //     recurrencePattern.ByMonthDay = new List<int> { microsoftRecurrence.Pattern.DayOfMonth };  // TODO: Check
        // }

        if (microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.RELATIVE_MONTHLY)
        {
            recurrencePattern.ByDay = microsoftRecurrence.Pattern.DaysOfWeek
                .Select(d => new WeekDay
                {
                    DayOfWeek = d.ToICalDayOfWeek(), 
                    Offset = microsoftRecurrence.Pattern.Index.ToInt()
                }).ToList();
        }

        // if (microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.ABSOLUTE_YEARLY)
        // {
        //     recurrencePattern.ByMonthDay = new List<int> { microsoftRecurrence.Pattern.DayOfMonth };// TODO: Check
        //     recurrencePattern.ByMonth = new List<int> { microsoftRecurrence.Pattern.Month };
        // }

        if (microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.RELATIVE_YEARLY)
        {
            //recurrencePattern.ByMonth = new List<int> { microsoftRecurrence.Pattern.Month };// TODO: Check
            recurrencePattern.ByDay = microsoftRecurrence.Pattern.DaysOfWeek
                .Select(d => new WeekDay
                {
                    DayOfWeek = d.ToICalDayOfWeek(), 
                    Offset = microsoftRecurrence.Pattern.Index.ToInt()
                }).ToList();
        }

        if (microsoftRecurrence.Range.Type == MicrosoftRecurrenceRangeType.END_DATE)
        {
            recurrencePattern.Until = DateTime.SpecifyKind(microsoftRecurrence.Range.EndDate, DateTimeKind.Utc);
        }

        if (microsoftRecurrence.Range.Type == MicrosoftRecurrenceRangeType.NUMBERED)
        {
            recurrencePattern.Count = microsoftRecurrence.Range.NumberOfOccurrences;
        }

        return $"RRULE:{recurrencePattern.ToString()}";
    }

    public static bool IsValid(this MicrosoftRecurrence microsoftRecurrence)
    {
        return
            microsoftRecurrence.Pattern.Interval != 0 && (
                microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.DAILY ||
                (
                    microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.WEEKLY && 
                    microsoftRecurrence.Pattern.DaysOfWeek != null && 
                    microsoftRecurrence.Pattern.DaysOfWeek.Length > 0
                ) || 
                (
                    microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.ABSOLUTE_MONTHLY &&
                    microsoftRecurrence.Pattern.DayOfMonth != 0
                ) ||
                (
                    microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.RELATIVE_MONTHLY &&
                    microsoftRecurrence.Pattern.DaysOfWeek != null &&
                    microsoftRecurrence.Pattern.DaysOfWeek.Length > 0 &&
                    microsoftRecurrence.Pattern.Index != null
                ) || 
                (
                    microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.ABSOLUTE_YEARLY &&
                    microsoftRecurrence.Pattern.Month != 0 && 
                    microsoftRecurrence.Pattern.DayOfMonth != 0
                ) || 
                (
                    microsoftRecurrence.Pattern.Type == MicrosoftRecurrencePatternType.RELATIVE_YEARLY &&
                    microsoftRecurrence.Pattern.Month != 0 &&
                    microsoftRecurrence.Pattern.DaysOfWeek != null &&
                    microsoftRecurrence.Pattern.DaysOfWeek.Length > 0 &&
                    microsoftRecurrence.Pattern.Index != null
                )
            ) && 
            (
                microsoftRecurrence.Range.Type == MicrosoftRecurrenceRangeType.NO_END ||
                (
                    microsoftRecurrence.Range.Type == MicrosoftRecurrenceRangeType.END_DATE && 
                    microsoftRecurrence.Range.EndDate > DateTime.MinValue && 
                    microsoftRecurrence.Range.EndDate > microsoftRecurrence.Range.StartDate
                ) || 
                (
                    microsoftRecurrence.Range.Type == MicrosoftRecurrenceRangeType.NUMBERED &&
                    microsoftRecurrence.Range.NumberOfOccurrences > 0
                )
            );
    }

    public static MicrosoftRecurrence ToMicrosoftRecurrence(this string recurrencePatternStr, DateTime startDate)
    {
        if (recurrencePatternStr == null)
            return null;
        
        var recurrencePattern = new RecurrencePattern(recurrencePatternStr);

        MicrosoftRecurrenceRangeType rangeType;
        if (recurrencePattern.Count > 0)
            rangeType = MicrosoftRecurrenceRangeType.NUMBERED;
        else if (recurrencePattern.Until != default)
            rangeType = MicrosoftRecurrenceRangeType.END_DATE;
        else
            rangeType = MicrosoftRecurrenceRangeType.NO_END;

        var range = new MicrosoftRecurrenceRange
        {
            Type = rangeType,
            StartDate = startDate,
            EndDate = recurrencePattern.Until,
            NumberOfOccurrences = recurrencePattern.Count > 0 ? recurrencePattern.Count : 0
        };

        var pattern = new MicrosoftRecurrencePattern();
        pattern.Type = recurrencePattern.Frequency.ToMicrosoftRecurrencePatternType(recurrencePattern);
        pattern.Interval = recurrencePattern.Interval;
        pattern.DaysOfWeek = pattern.Type is
            MicrosoftRecurrencePatternType.WEEKLY or
            MicrosoftRecurrencePatternType.RELATIVE_MONTHLY or
            MicrosoftRecurrencePatternType.RELATIVE_YEARLY
            ? (recurrencePattern.ByDay.Count > 0 
                ? recurrencePattern.ByDay.Select(wd => wd.DayOfWeek.ToMicrosoftDayOfWeek()).ToArray()
                : new MicrosoftDayOfWeek[] { startDate.DayOfWeek.ToMicrosoftDayOfWeek()})
            : new MicrosoftDayOfWeek[] {};
        pattern.DayOfMonth = pattern.Type is
            MicrosoftRecurrencePatternType.ABSOLUTE_MONTHLY or
            MicrosoftRecurrencePatternType.ABSOLUTE_YEARLY
            ? startDate.Day
            : 0;
        pattern.Index = pattern.Type is
            MicrosoftRecurrencePatternType.RELATIVE_MONTHLY or
            MicrosoftRecurrencePatternType.RELATIVE_YEARLY
            ? GetMicrosoftWeekIndex(recurrencePattern.ByDay.FirstOrDefault().Offset)
            : MicrosoftWeekIndex.FIRST;
        pattern.Month = pattern.Type is
            MicrosoftRecurrencePatternType.ABSOLUTE_YEARLY or
            MicrosoftRecurrencePatternType.RELATIVE_YEARLY
            ? startDate.Month
            : 0;

        var microsoftRecurrence = new MicrosoftRecurrence
        {
            Pattern = pattern,
            Range = range
        };

        return microsoftRecurrence;
    }
    
    public static FrequencyType ToICalFrequencyType(this MicrosoftRecurrencePatternType type)
    {
        switch (type)
        {
            case MicrosoftRecurrencePatternType.DAILY:
                return FrequencyType.Daily;
            case MicrosoftRecurrencePatternType.WEEKLY:
                return FrequencyType.Weekly;
            case MicrosoftRecurrencePatternType.ABSOLUTE_MONTHLY:
            case MicrosoftRecurrencePatternType.RELATIVE_MONTHLY:
                return FrequencyType.Monthly;
            case MicrosoftRecurrencePatternType.ABSOLUTE_YEARLY:
            case MicrosoftRecurrencePatternType.RELATIVE_YEARLY:
                return FrequencyType.Yearly;
            default:
                return FrequencyType.None;
        }
    }

    // TODO: handle absolute - relative 
    public static MicrosoftRecurrencePatternType ToMicrosoftRecurrencePatternType(
        this FrequencyType type, 
        RecurrencePattern recurrencePattern)
    {
        return type switch
        {
            FrequencyType.Daily => MicrosoftRecurrencePatternType.DAILY,
            FrequencyType.Weekly => MicrosoftRecurrencePatternType.WEEKLY,
            FrequencyType.Monthly => recurrencePattern.ByDay.Count > 0 
                ? MicrosoftRecurrencePatternType.RELATIVE_MONTHLY 
                : MicrosoftRecurrencePatternType.ABSOLUTE_MONTHLY,
            FrequencyType.Yearly => recurrencePattern.ByDay.Count > 0
                ? MicrosoftRecurrencePatternType.RELATIVE_YEARLY
                : MicrosoftRecurrencePatternType.ABSOLUTE_YEARLY,
            _ => MicrosoftRecurrencePatternType.DAILY
        };
    }

    public static DayOfWeek ToICalDayOfWeek(this MicrosoftDayOfWeek microsoftDayOfWeek)
    {
        return microsoftDayOfWeek switch
        {
            MicrosoftDayOfWeek.MONDAY => DayOfWeek.Monday,
            MicrosoftDayOfWeek.TUESDAY => DayOfWeek.Tuesday,
            MicrosoftDayOfWeek.WEDNESDAY => DayOfWeek.Wednesday,
            MicrosoftDayOfWeek.THURSDAY => DayOfWeek.Thursday,
            MicrosoftDayOfWeek.FRIDAY => DayOfWeek.Friday,
            MicrosoftDayOfWeek.SATURDAY => DayOfWeek.Saturday,
            MicrosoftDayOfWeek.SUNDAY => DayOfWeek.Sunday,
            _ => DayOfWeek.Monday
        };
    }
    
    public static MicrosoftDayOfWeek ToMicrosoftDayOfWeek(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => MicrosoftDayOfWeek.MONDAY,
            DayOfWeek.Tuesday => MicrosoftDayOfWeek.TUESDAY,
            DayOfWeek.Wednesday => MicrosoftDayOfWeek.WEDNESDAY,
            DayOfWeek.Thursday => MicrosoftDayOfWeek.THURSDAY, 
            DayOfWeek.Friday => MicrosoftDayOfWeek.FRIDAY,
            DayOfWeek.Saturday => MicrosoftDayOfWeek.SATURDAY,
            DayOfWeek.Sunday => MicrosoftDayOfWeek.SUNDAY,
            _ => MicrosoftDayOfWeek.MONDAY
        };
    }

    public static int ToInt(this MicrosoftWeekIndex weekIndex)
    {
        return weekIndex switch
        { 
            MicrosoftWeekIndex.FIRST => 1,
            MicrosoftWeekIndex.SECOND => 2,
            MicrosoftWeekIndex.THIRD => 3, 
            MicrosoftWeekIndex.FOURTH => 4, 
            MicrosoftWeekIndex.LAST => 5, 
            _ => 1 
        };
    }

    private static MicrosoftWeekIndex GetMicrosoftWeekIndex(int offset)
    {
        return offset switch
        { 
            1 => MicrosoftWeekIndex.FIRST,
            2 => MicrosoftWeekIndex.SECOND,
            3 => MicrosoftWeekIndex.THIRD, 
            4 => MicrosoftWeekIndex.FOURTH, 
            5 => MicrosoftWeekIndex.LAST, 
            _ => MicrosoftWeekIndex.FIRST
        };
    }
}