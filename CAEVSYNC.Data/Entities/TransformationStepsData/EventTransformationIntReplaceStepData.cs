using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationIntReplaceStepData
{
    [Key]
    public string Id { get; set; }
    
    public int IntReplacement { get; set; }
    
    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}