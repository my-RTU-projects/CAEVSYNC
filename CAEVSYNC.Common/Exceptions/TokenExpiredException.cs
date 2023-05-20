using CAEVSYNC.Common.Models.Enums;

namespace CAEVSYNC.Common.Exceptions;

public class TokenExpiredException : Exception
{
    public AccountType AccountType { get; }
    
    public string AccountId { get; }

    public TokenExpiredException(AccountType accountType, string accountId) 
        : this("", null, accountType, accountId) { }

    public TokenExpiredException(string message, AccountType accountType, string accountId) 
        : this(message, null, accountType, accountId) { }

    public TokenExpiredException(string message, Exception inner, AccountType accountType, string accountId)
        : base(message, inner)
    {
        this.AccountType = accountType;
        this.AccountId = accountId;
    }
}