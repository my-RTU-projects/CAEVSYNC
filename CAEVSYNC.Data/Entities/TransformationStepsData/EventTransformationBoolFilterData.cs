using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class EventTransformationBoolFilterData
{
    [Key]
    public string Id { get; set; }

    public bool BoolFilter { get; set; }

    [ForeignKey(nameof(Id))]
    public EventTransformationStep EventTransformationStep { get; set; }
}