namespace CAEVSYNC.Common.Models;

public class MicrosoftRecurrencePattern
{
    public MicrosoftRecurrencePatternType Type { get; set; }
    
    public int DayOfMonth { get; set; }
    
    public MicrosoftDayOfWeek[] DaysOfWeek { get; set; }
    
    public MicrosoftDayOfWeek FirstDayOfWeek { get; set; }
    
    public MicrosoftWeekIndex Index { get; set; }
    
    public int Interval { get; set; }
    
    public int Month { get; set; }
}