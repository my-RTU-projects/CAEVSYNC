using CAEVSYNC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAEVSYNC.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CalendarSyncController : ControllerBase
{
    private readonly CalendarSyncService _eventSyncService;
    private readonly UserManager<IdentityUser> _userManager;
    
    public CalendarSyncController(CalendarSyncService eventSyncService, UserManager<IdentityUser> userManager)
    {
        _eventSyncService = eventSyncService;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<ActionResult> SyncCalendarsAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        //await _eventSyncService.SyncEventsAsync(user.Id);
        await _eventSyncService.StartSynchronizationAsync(user.Id); 
        
        return Ok();
    }
    
    [HttpGet("isActive")]
    public async Task<ActionResult<bool>> CheckIsSyncJobActiveAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var isActive = await _eventSyncService.CheckIsSyncJobActiveAsync(user.Id); 
        
        return Ok(isActive);
    }
}