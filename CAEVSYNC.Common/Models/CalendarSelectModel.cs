namespace CAEVSYNC.Common.Models;

public class CalendarSelectModel
{
    public string CalendarIdByProvider { get; set; }
    
    public string Title { get; set; }
    
    public string Account { get; set; }
    
    public bool ReadOnly { get; set; }
    
    public string ColorHex { get; set; }
}