using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAEVSYNC.Data.Entities;

public class Calendar
{
    [Key]
    [MaxLength(200)]
    public string Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(8)]
    public string ColorHex { get; set; }

    [InverseProperty(nameof(AccountToCalendarConnection.Calendar))]
    public List<AccountToCalendarConnection> AccountToCalendarConnections { get; set; } = new();
}