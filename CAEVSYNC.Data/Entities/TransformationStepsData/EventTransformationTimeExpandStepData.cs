using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationTimeExpandStepData
{
    [Key]
    public string Id { get; set; }
    
    public int? ExtraMinutesBefore { get; set; }
    
    public int? ExtraMinutesAfter { get; set; }
    
    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}