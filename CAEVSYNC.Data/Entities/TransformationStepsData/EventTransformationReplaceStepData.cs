using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationReplaceStepData
{
    [Key]
    public string Id { get; set; }
    
    public int? IntReplacement { get; set; }
    
    [MaxLength(2000)]
    public string? StringReplacement { get; set; }
    
    public bool? BoolReplacement { get; set; }
    
    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}