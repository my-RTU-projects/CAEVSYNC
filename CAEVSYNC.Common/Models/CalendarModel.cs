namespace CAEVSYNC.Common.Models;

public class CalendarModel
{
    public string CalendarIdByProvider { get; set; }
    
    public string Title { get; set; }
    
    public bool ReadOnly { get; set; }
    
    public string ColorHex { get; set; }
}