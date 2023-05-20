using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CAEVSYNC.Data.Entities;

[PrimaryKey(nameof(UserId), nameof(ConnectedAccountId))]
public class UserToAccountConnection
{
    [MaxLength(200)]
    public string UserId { get; set; }
    
    [MaxLength(200)]
    public string ConnectedAccountId { get; set; }

    [ForeignKey(nameof(ConnectedAccountId))]
    public ConnectedAccount? ConnectedAccount { get; set; }
}