using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CAEVSYNC.Data.Entities;

[PrimaryKey(nameof(AccountId), nameof(CalendarId))]
public class AccountToCalendarConnection
{
    [MaxLength(200)]
    public string AccountId { get; set; }
    
    [MaxLength(200)]
    public string CalendarId { get; set; }
    
    [Required]
    public bool ReadOnly { get; set; }

    [ForeignKey(nameof(CalendarId))]
    public Calendar? Calendar { get; set; }
    
    [ForeignKey(nameof(AccountId))]
    public ConnectedAccount? Account { get; set; }
}