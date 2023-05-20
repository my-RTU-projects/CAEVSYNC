namespace CAEVSYNC.Common.Models;

public class SyncRuleCreateModel
{
    public string Title { get; set; }
    
    public string OriginCalendarId { get; set; }
    
    public string TargetCalendarId { get; set; }
}