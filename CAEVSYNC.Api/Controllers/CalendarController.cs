using CAEVSYNC.Common.Models;
using CAEVSYNC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAEVSYNC.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly CalendarService _calendarService;
    private readonly UserManager<IdentityUser> _userManager;

    public CalendarController(CalendarService calendarService, UserManager<IdentityUser> userManager)
    {
        _calendarService = calendarService;
        _userManager = userManager;
    }   
    
    [HttpGet("list/{accountId}")]
    public async Task<ActionResult<List<CalendarSelectModel>>> GetCalendarsAsync(string accountId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var calendars = await _calendarService.GetCalendarsAsync(user.Id, accountId);
        
        return Ok(calendars);
    }
    
    [HttpGet("list/all")]
    public async Task<ActionResult<List<CalendarSelectModel>>> GetAllCalendarsAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var calendars = await _calendarService.GetCalendarsAsync(user.Id);
        
        return Ok(calendars);
    }
    
    [HttpGet("renderModel/{calendarId}")]
    public async Task<ActionResult<CalendarRenderModel>> GetCalendarRenderModelAsync(string calendarId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var calendar = await _calendarService.GetCalendarRenderModelAsync(user.Id, calendarId);
        
        return Ok(calendar);
    }
}