using CAEVSYNC.ConnectedAccounts.Auth.Models;

namespace CAEVSYNC.ConnectedAccounts.TokenDataStores;

public interface ITokenDataStore
{
    Task SaveTokenDataAsync(string key, AuthTokens authTokens);

    Task DeleteTokenAsync(string key);
    
    Task<AuthTokens> GetTokenDataAsync(string key);

    Task ClearAsync();
}