namespace CAEVSYNC.Common.Models;

public class MicrosoftEventModel
{
    public string Subject { get; set; }
    
    public MicrosoftEventBodyModel Body { get; set; }
    
    public MicrosoftEventDateTimeModel Start { get; set; }
    
    public MicrosoftEventDateTimeModel End { get; set; }
    
    public MicrosoftEventLocationModel Location { get; set; }

    public bool IsReminderOn { get; set; } = false;

    public int ReminderMinutesBeforeStart { get; set; } = 0;
    
    public MicrosoftRecurrence? Recurrence { get; set; }

    public bool IsAllDay { get; set; } = false;
}