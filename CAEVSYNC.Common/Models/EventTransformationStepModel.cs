using CAEVSYNC.Common.Models.Enums;

namespace CAEVSYNC.Common.Models;

public class EventTransformationStepModel
{
    public string Id { get; set; }
    
    public string PropertyName { get; set; }
    
    public PropertyType PropertyType { get; set; }
    
    public EventTransformationType TransformationType { get; set; }
    
    public int? IntReplacement { get; set; }
    
    public string? StringReplacement { get; set; }
    
    public bool? BoolReplacement { get; set; }
    
    public int? IntFilter { get; set; }
    
    public string? StringFilter { get; set; }
    
    public bool? BoolFilter { get; set; }
    
    public DateTime? FromDateTime { get; set; }
    
    public DateTime? ToDateTime { get; set; }

    public int? ExtraMinutesBefore { get; set; }
    
    public int? ExtraMinutesAfter { get; set; }
}