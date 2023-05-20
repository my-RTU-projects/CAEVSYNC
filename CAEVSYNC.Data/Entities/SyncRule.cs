using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class SyncRule
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string UserId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string OriginCalendarId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string TargetCalendarId { get; set; }

    [ForeignKey(nameof(OriginCalendarId))]
    public Calendar? OriginCalendar { get; set; }
    
    [ForeignKey(nameof(TargetCalendarId))]
    public Calendar? TargetCalendar { get; set; }
    
    [InverseProperty(nameof(EventTransformationStep.SyncRule))]
    public List<EventTransformationStep> EventTransformationSteps { get; set; } = new();
}