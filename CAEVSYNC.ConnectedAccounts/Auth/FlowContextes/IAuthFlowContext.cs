using CAEVSYNC.ConnectedAccounts.Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;

public interface IAuthFlowContext
{
    string GetAuthCodeRequestUrl(string userId);

    Task<RedirectResult> HandleAuthCodeAsync(string? code, string? error, string? state);

    Task RefreshAccessToken(string userId, string accountId);

    Task<AuthTokens> GetTokensAsync(string userId, string accountId);
}