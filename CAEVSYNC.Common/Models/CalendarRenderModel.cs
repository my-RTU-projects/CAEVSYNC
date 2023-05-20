using CAEVSYNC.Common.Models.Enums;

namespace CAEVSYNC.Common.Models;

public class CalendarRenderModel
{
    public string Id { get; set; }
    
    public string Title { get; set; }
    
    public string AccountName { get; set; }
    
    public AccountType AccountType { get; set; }
    
    public string ColorHex { get; set; }
}