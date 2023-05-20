namespace CAEVSYNC.Common.Models;

public class SyncRuleModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string OriginCalendarId { get; set; }
    
    public string TargetCalendarId { get; set; }
    
    public string OriginCalendarTitle { get; set; }
    
    public string TargetCalendarTitle { get; set; }
    
    public List<EventTransformationStepModel> EventTransformationSteps { get; set; } = new();
}