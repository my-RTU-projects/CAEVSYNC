using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAEVSYNC.Common.Models.Enums;

namespace CAEVSYNC.Data.Entities;

public class ConnectedAccount
{
    [Key]
    [MaxLength(200)]
    public string Id { get; set; }

    [Required]
    public AccountType AccountType { get; set; }
    
    [Required]
    public AccountStatus AccountStatus { get; set; }
    
    public string Title { get; set; }

    [InverseProperty(nameof(UserToAccountConnection.ConnectedAccount))]
    public List<UserToAccountConnection> UserToAccountConnections { get; set; } = new();
    
    [InverseProperty(nameof(AccountToCalendarConnection.Account))]
    public List<AccountToCalendarConnection> AccountToCalendarConnections { get; set; } = new();
}