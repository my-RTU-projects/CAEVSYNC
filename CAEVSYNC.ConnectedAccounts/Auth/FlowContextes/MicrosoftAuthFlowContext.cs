using System.Net;
using System.Security.Authentication;
using CAEVSYNC.Common.Models.Enums;
using CAEVSYNC.Auth.Services;
using CAEVSYNC.Common.Models;
using CAEVSYNC.ConnectedAccounts.Auth.Models;
using CAEVSYNC.ConnectedAccounts.TokenDataStores;
using CAEVSYNC.Data;
using CAEVSYNC.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;

public class MicrosoftAuthFlowContext : IAuthFlowContext
{
    private readonly string _baseUrl;
    
    private readonly ITokenDataStore _dataStore;
    private readonly CaevsyncDbContext _dbContext;
    private readonly UserService _userService;

    public MicrosoftAuthFlowContext(
        CaevsyncDbContext dbContext, 
        UserService userService)
    {
        _baseUrl = "https://login.microsoftonline.com/common/oauth2/v2.0";
        
        _dataStore = new FileTokenDataStore("microsoft-auth-tokens");
        _dbContext = dbContext;
        _userService = userService;
    }
    
    public string GetAuthCodeRequestUrl(string userId)
    {
        var codeRequestUrl = $"{_baseUrl}/authorize?" +
                             $"client_id={Uri.EscapeDataString(Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_ID"))}" +
                             "&response_type=code" +
                             $"&redirect_uri={Uri.EscapeDataString(Environment.GetEnvironmentVariable("MICROSOFT_AUTH_REDIRECT_URL"))}" +
                             $"&scope={Uri.EscapeDataString(Environment.GetEnvironmentVariable("MICROSOFT_SCOPES"))}" +
                             "&response_mode=query" +
                             $"&state={Uri.EscapeDataString(userId)}";

        return codeRequestUrl;
    }

    public async Task<RedirectResult> HandleAuthCodeAsync(string? code, string? error, string? state)
    {
        if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
            throw new AuthenticationException($"Error during authorization: {error}");

        await _userService.ThrowIfUserNotExistAsync(state);
        
        RestClient restClient = new RestClient(new Uri($"{_baseUrl}/token"));
        RestRequest restRequest = new RestRequest();

        restRequest.AddParameter("client_id", Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_ID"));
        restRequest.AddParameter("scope", Environment.GetEnvironmentVariable("MICROSOFT_SCOPES"));
        restRequest.AddParameter("code", code);
        restRequest.AddParameter("redirect_uri", Environment.GetEnvironmentVariable("MICROSOFT_AUTH_REDIRECT_URL"));
        restRequest.AddParameter("grant_type", "authorization_code");
        restRequest.AddParameter("client_secret", Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_SECRET"));

        var response = await restClient.PostAsync(restRequest);
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new AuthenticationException($"Error during authorization: {response.ErrorMessage}");

        var responseJObject = JObject.Parse(response.Content);

        var authTokens = new AuthTokens
        {
            AccessToken = responseJObject["access_token"].ToString(),
            RefreshToken = responseJObject["refresh_token"].ToString()
        };
        
        var accountData = await GetMicrosoftAccountData(authTokens);
        
        var account = await _dbContext.ConnectedAccounts.FindAsync(accountData.AccountId) ?? new ConnectedAccount()
        {
            Id = accountData.AccountId, 
            Title = accountData.AccountTitle,
            AccountType = AccountType.MICROSOFT
        };

        account.AccountStatus = AccountStatus.ACTIVE;
        
        var userToAccountConnection = await _dbContext.UserToAccountConnections.FindAsync(state, accountData.AccountId);

        if (userToAccountConnection == null)
        {
            userToAccountConnection = new UserToAccountConnection
            {
                UserId = state,
                ConnectedAccount = account
            };
            _dbContext.UserToAccountConnections.Add(userToAccountConnection);
        }
        
        await _dbContext.SaveChangesAsync();
        
        await _dataStore.SaveTokenDataAsync($"{state}-{accountData.AccountId}", authTokens);

        return new RedirectResult(Environment.GetEnvironmentVariable("REACT_CLIENT_URL"));
    }

    public async Task RefreshAccessToken(string userId, string accountId)
    {
        var tokens = await _dataStore.GetTokenDataAsync($"{userId}-{accountId}");
        
        RestClient restClient = new RestClient(new Uri($"{_baseUrl}/token"));
        RestRequest restRequest = new RestRequest();

        restRequest.AddParameter("client_id", Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_ID"));
        restRequest.AddParameter("scope", Environment.GetEnvironmentVariable("MICROSOFT_SCOPES"));
        restRequest.AddParameter("refresh_token", tokens.RefreshToken);
        restRequest.AddParameter("grant_type", "refresh_token");
        restRequest.AddParameter("client_secret", Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_SECRET"));

        var response = await restClient.PostAsync(restRequest);
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new AuthenticationException($"Error during authorization: {response.ErrorMessage}");
        
        var responseJObject = JObject.Parse(response.Content);

        var authTokens = new AuthTokens
        {
            AccessToken = responseJObject["access_token"].ToString(),
            RefreshToken = responseJObject["refresh_token"].ToString()
        };

        _dataStore.SaveTokenDataAsync($"{userId}-{accountId}", authTokens);
    }

    public async Task<AuthTokens> GetTokensAsync(string userId, string accountId)
    {
        var tokens = await _dataStore.GetTokenDataAsync($"{userId}-{accountId}");
        return tokens;
    }

    private async Task<AccountData> GetMicrosoftAccountData(AuthTokens tokens)
    {
        RestClient restClient = new RestClient(new Uri("https://graph.microsoft.com/v1.0/me"));
        RestRequest restRequest = new RestRequest();

        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.GetAsync(restRequest);
        var responseJObject = JObject.Parse(response.Content);
        var accountId = responseJObject["id"].ToString();
        var accountTitle = responseJObject["userPrincipalName"].ToString();

        return new AccountData
        {
            AccountId = accountId,
            AccountTitle = accountTitle
        };
    }
}