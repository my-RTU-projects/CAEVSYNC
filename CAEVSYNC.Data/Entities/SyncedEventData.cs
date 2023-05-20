using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class SyncedEventData
{
    [Key]
    public int Id { get; set; }
    
    public int SyncRuleId { get; set; }
    
    [ForeignKey(nameof(SyncRuleId))]
    public SyncRule? SyncRule { get; set; }
    
    public string EventIdInOriginCalendar { get; set; }

    public string EventIdInTargetCalendarId { get; set; }
}