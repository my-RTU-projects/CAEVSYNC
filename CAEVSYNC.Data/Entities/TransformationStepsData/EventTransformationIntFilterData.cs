using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationIntFilterData
{
    [Key]
    public string Id { get; set; }
    
    public int IntFilter { get; set; }

    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}