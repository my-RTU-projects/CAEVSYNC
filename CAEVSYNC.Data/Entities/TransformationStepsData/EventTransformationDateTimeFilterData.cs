using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationDateTimeFilterData
{
    [Key]
    public string Id { get; set; }

    public DateTime? FromDateTime { get; set; }
    
    public DateTime? ToDateTime { get; set; }
    
    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}