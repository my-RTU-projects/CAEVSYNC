using CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;
using CAEVSYNC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAEVSYNC.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MicrosoftAuthController : ControllerBase 
{
    private readonly MicrosoftAuthFlowContext _authContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly CalendarSyncService _calendarSyncService;
    
    public MicrosoftAuthController(
        MicrosoftAuthFlowContext authContext, 
        UserManager<IdentityUser> userManager,
        CalendarSyncService calendarSyncService)
    {
        _authContext = authContext;
        _userManager = userManager;
        _calendarSyncService = calendarSyncService;
    }
    
    [Authorize]
    [HttpGet("authCodeRequest")]
    public ActionResult<string> GetMicrosoftAuthCodeRequestUrl()
    {
        var userId = _userManager.GetUserId(HttpContext.User);
        return _authContext.GetAuthCodeRequestUrl(userId);
    }
    
    [HttpGet("callback")]
    public async Task<RedirectResult> CodeCallbackAsync([FromQuery] string? code, [FromQuery] string? state, [FromQuery] string? error)
    {
        var redirectResult = await _authContext.HandleAuthCodeAsync(code, error, state);
        
        await _calendarSyncService.StartSynchronizationAsync(state);
        
        return redirectResult;
    }
}