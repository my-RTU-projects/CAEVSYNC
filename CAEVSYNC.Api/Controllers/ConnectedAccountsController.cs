using CAEVSYNC.Common.Models;
using CAEVSYNC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAEVSYNC.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ConnectedAccountsController : ControllerBase
{
    private readonly ConnectedAccountService _connectedAccountService;
    private readonly UserManager<IdentityUser> _userManager;

    public ConnectedAccountsController(
        ConnectedAccountService connectedAccountService, 
        UserManager<IdentityUser> userManager)
    {
        _connectedAccountService = connectedAccountService;
        _userManager = userManager;
    }   
    
    [HttpGet("list")]
    public async Task<ActionResult<List<ConnectedAccountModel>>> GetConnectedAccountAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var accountModels = await _connectedAccountService.GetConnectedAccountsAsync(user.Id);
        
        return Ok(accountModels);
    }

    [HttpDelete("{accountId}")]
    public async Task<ActionResult<Task>> DeleteAccountAsync(string accountId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _connectedAccountService.DeleteAccountAsync(user.Id, accountId);
        
        return Ok();
    }
}