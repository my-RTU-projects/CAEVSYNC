using CAEVSYNC.Common.Models.Enums;

namespace CAEVSYNC.Common.Models;

public class ConnectedAccountModel
{
    public string Id { get; set; }

    public string Title { get; set; }
    
    public AccountType AccountType { get; set; }
    
    public AccountStatus AccountStatus { get; set; }
}