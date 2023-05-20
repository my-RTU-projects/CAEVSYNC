using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CAEVSYNC.Auth.Services;

public class UserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IServiceProvider _serviceProvider;
    
    public UserService(
        IHttpContextAccessor contextAccessor,
        IServiceProvider serviceProvider)
    {
        _contextAccessor = contextAccessor;
        _serviceProvider = serviceProvider;
    }
    
    public async Task ThrowIfUserNotExistAsync(string userId)
    {
        var userManager = _serviceProvider.GetService<UserManager<IdentityUser>>();
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) 
            throw new AuthenticationException($"User with such id doesn't exist");
    }
}