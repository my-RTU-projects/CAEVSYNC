using System.Net;
using System.Security.Authentication;
using CAEVSYNC.Common.Models.Enums;
using CAEVSYNC.Auth.Services;
using CAEVSYNC.Common.Exceptions;
using CAEVSYNC.ConnectedAccounts.Auth.Models;
using CAEVSYNC.ConnectedAccounts.TokenDataStores;
using CAEVSYNC.Data;
using CAEVSYNC.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;

// TODO: Если приложение потеряло права доступа или токены утеряны, то кидать исключение, ловить на более высоком уровне и отправлять уведомление о проблеме
// TODO: 
public class GoogleAuthFlowContext : IAuthFlowContext
{
    private readonly string _baseCodeUrl;
    private readonly string _baseTokenUrl;
    
    private readonly ITokenDataStore _dataStore;
    private readonly CaevsyncDbContext _dbContext;
    private readonly UserService _userService;
    
    public GoogleAuthFlowContext(
        CaevsyncDbContext dbContext, 
        UserService userService)
    {
        _baseCodeUrl = "https://accounts.google.com/o/oauth2/v2";
        _baseTokenUrl = "https://oauth2.googleapis.com";
        
        _dataStore = new FileTokenDataStore("google-auth-tokens");
        _dbContext = dbContext;
        _userService = userService;
    }
    
    public string GetAuthCodeRequestUrl(string userId)
    {
        var codeRequestUrl = $"{_baseCodeUrl}/auth?" +
                             $"client_id={Uri.EscapeDataString(Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"))}" +
                             $"&redirect_uri={Uri.EscapeDataString(Environment.GetEnvironmentVariable("GOOGLE_AUTH_CODE_REDIRECT_URL"))}" +
                             "&response_type=code" +
                             $"&scope={Uri.EscapeDataString(Environment.GetEnvironmentVariable("GOOGLE_SCOPES"))}" +
                             "&prompt=consent" + 
                             "&access_type=offline" +
                             "&include_granted_scopes=true" +
                             $"&state={Uri.EscapeDataString(userId)}";

        return codeRequestUrl;
    }

    public async Task<RedirectResult> HandleAuthCodeAsync(string? code, string? error, string? state)
    {
        if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(code))
            throw new AuthenticationException($"Error during authorization: {error}");

        await _userService.ThrowIfUserNotExistAsync(state);
        
        RestClient restClient = new RestClient(new Uri($"{_baseTokenUrl}/token"));
        RestRequest restRequest = new RestRequest();

        restRequest.AddParameter("client_id", Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"));
        restRequest.AddParameter("client_secret", Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET"));
        restRequest.AddParameter("code", code);
        restRequest.AddParameter("grant_type", "authorization_code");
        restRequest.AddParameter("redirect_uri", Environment.GetEnvironmentVariable("GOOGLE_AUTH_CODE_REDIRECT_URL"));

        var response = await restClient.PostAsync(restRequest);
        
        if (response.StatusCode != HttpStatusCode.OK)
            throw new AuthenticationException($"Error during authorization: {response.ErrorMessage}");

        var responseJObject = JObject.Parse(response.Content);

        var authTokens = new AuthTokens
        {
            AccessToken = responseJObject["access_token"].ToString(),
            RefreshToken = responseJObject["refresh_token"].ToString()
        };
        
        var accountData = await GetGoogleAccountDataAsync(authTokens);
        var account = await _dbContext.ConnectedAccounts.FindAsync(accountData.AccountId) ?? new ConnectedAccount()
        {
            Id = accountData.AccountId,
            Title = accountData.AccountTitle,
            AccountType = AccountType.GOOGLE
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
        
        RestClient restClient = new RestClient(new Uri($"{_baseTokenUrl}/token"));
        RestRequest restRequest = new RestRequest();

        restRequest.AddParameter("client_id", Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"));
        restRequest.AddParameter("client_secret", Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET"));
        restRequest.AddParameter("grant_type", "refresh_token");
        restRequest.AddParameter("refresh_token", tokens.RefreshToken);
        
        var response = await restClient.ExecutePostAsync(restRequest);
        
        var responseJObject = JObject.Parse(response.Content);
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            var account = await _dbContext.ConnectedAccounts.FindAsync(accountId);
            
            var error = responseJObject["error"]?.ToString();
            
            if (error.Equals("invalid_grant"))
            {
                account.AccountStatus = AccountStatus.TOKEN_EXPIRED;
                throw new TokenExpiredException(AccountType.GOOGLE, accountId);
            }
            
            account.AccountStatus = AccountStatus.OBSCURE_ERROR;
            throw new AuthenticationException($"Error during authorization: {response.ErrorMessage}");
        }

        var authTokens = new AuthTokens
        {
            AccessToken = responseJObject["access_token"].ToString(),
            RefreshToken = tokens.RefreshToken
        };

        _dataStore.SaveTokenDataAsync($"{userId}-{accountId}", authTokens);
    }

    public async Task<AuthTokens> GetTokensAsync(string userId, string accountId)
    {
        var tokens = await _dataStore.GetTokenDataAsync($"{userId}-{accountId}");
        return tokens;
    }

    private async Task<AccountData> GetGoogleAccountDataAsync(AuthTokens tokens)
    {
        RestClient restClient = new RestClient(new Uri("https://www.googleapis.com/oauth2/v3/userinfo"));
        RestRequest restRequest = new RestRequest();
            
        restRequest.AddHeader("Authorization", $"Bearer {tokens.AccessToken}");

        var response = await restClient.GetAsync(restRequest);
        var responseJObject = JObject.Parse(response.Content);
        var accountId = responseJObject["sub"].ToString();
        var accountTitle = responseJObject["email"].ToString();
        
        return new AccountData
        {
            AccountId = accountId,
            AccountTitle = accountTitle
        };
    }
}