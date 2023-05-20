using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationStringFilterData
{
    [Key]
    public string Id { get; set; }
    
    [MaxLength(2000)]
    public string StringFilter { get; set; }

    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}