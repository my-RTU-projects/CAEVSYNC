namespace CAEVSYNC.Common.Models;

public class SyncRuleEditModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public List<EventTransformationStepModel> EventTransformationSteps { get; set; } = new();
}