using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationBoolReplaceStepData
{
    [Key]
    public string Id { get; set; }

    public bool BoolReplacement { get; set; }
    
    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}