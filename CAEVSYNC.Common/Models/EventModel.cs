namespace CAEVSYNC.Common.Models;

public class EventModel
{
    public string? EventIdByProvider { get; set; }
    
    public string CalendarIdByProvider { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime? FromDateTime { get; set; }

    public DateTime? ToDateTime { get; set; }
    
    public string? FromDateTimeZone { get; set; }
    
    public string? ToDateTimeZone { get; set; }
    
    public string? Location { get; set; }
    
    public bool IsReminderOn { get; set; }
    
    public int ReminderMinutesBeforeStart { get; set; }
    
    public bool IsAllDay { get; set; }
    
    // https://www.rfc-editor.org/rfc/rfc5545#section-3.8.5
    public string? RecurrencePattern { get; set; }
}