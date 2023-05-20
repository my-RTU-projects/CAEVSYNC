using CAEVSYNC.ConnectedAccounts.Auth.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CAEVSYNC.ConnectedAccounts.TokenDataStores;

public class FileTokenDataStore : ITokenDataStore
{
    public string FolderPath { get; } 
    
    public FileTokenDataStore(string relativeFolderPath)
    {
        FolderPath = $"{Environment.CurrentDirectory}/{relativeFolderPath}";
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
    }
    
    public Task SaveTokenDataAsync(string key, AuthTokens authTokens)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key must have a value (for example, USER_ID-CONNECTED_ACCOUNT_ID)");

        var jsonString = JsonConvert.SerializeObject(authTokens);
        var filePath = Path.Combine(FolderPath, key);
        
        File.WriteAllText(filePath, jsonString);

        return Task.CompletedTask;
    }

    public Task DeleteTokenAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key must have a value (for example, USER_ID-CONNECTED_ACCOUNT_ID)");

        var filePath = Path.Combine(FolderPath, key);
        
        if (File.Exists(filePath))
            File.Delete(filePath);
        
        return Task.CompletedTask;
    }
    
    public Task<AuthTokens?> GetTokenDataAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key must have a value (for example, USER_ID-CONNECTED_ACCOUNT_ID)");

        var taskCompletionSource = new TaskCompletionSource<AuthTokens>();
        
        var filePath = Path.Combine(FolderPath, key);

        if (!File.Exists(filePath))
        {
            taskCompletionSource.SetResult(null);
            return taskCompletionSource.Task;
        }

        try
        {
            var jsonString = File.ReadAllText(filePath);
            var authTokens = JsonSerializer.Deserialize<AuthTokens>(jsonString);
            taskCompletionSource.SetResult(authTokens);
        }
        catch (Exception ex)
        {
            taskCompletionSource.SetException(ex);
        }

        return taskCompletionSource.Task;
    }

    public Task ClearAsync()
    {
        if (Directory.Exists(FolderPath))
        {
            Directory.Delete(FolderPath, true);
            Directory.CreateDirectory(FolderPath);
        }

        return Task.CompletedTask;
    }
}