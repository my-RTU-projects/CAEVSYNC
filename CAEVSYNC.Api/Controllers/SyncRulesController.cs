using CAEVSYNC.Common.Models;
using CAEVSYNC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAEVSYNC.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SyncRulesController : ControllerBase
{
    private readonly SyncRulesService _syncRulesService;
    private readonly UserManager<IdentityUser> _userManager;

    public SyncRulesController(SyncRulesService syncRulesService, UserManager<IdentityUser> userManager)
    {
        _syncRulesService = syncRulesService;
        _userManager = userManager;
    }

    [HttpGet("list/{calendarId}")]
    public async Task<ActionResult<List<SyncRuleListModel>>> GetSyncRulesAsync(string calendarId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var syncRules = await _syncRulesService.GetSyncRulesAsync(user.Id, calendarId);
        
        return Ok(syncRules);
    }
    
    [HttpGet("{syncRuleId}")]
    public async Task<ActionResult<SyncRuleModel>> GetSyncRuleAsync(int syncRuleId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var syncRules = await _syncRulesService.GetSyncRuleAsync(syncRuleId);
        
        return Ok(syncRules);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateSyncRule([FromBody] SyncRuleCreateModel syncRuleModel)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var id = await _syncRulesService.CreateSyncRuleAsync(user.Id, syncRuleModel);
        
        return Ok(id);
    }
    
    [HttpPost("edit")]
    public async Task<ActionResult> EditSyncRule([FromBody] SyncRuleEditModel syncRuleModel)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _syncRulesService.EditSyncRuleAsync(user.Id, syncRuleModel);
        
        return Ok();
    }
    
    [HttpDelete("delete/{ruleId}")]
    public async Task<ActionResult> DeleteSyncRule(int ruleId)
    {
        await _syncRulesService.DeleteSyncRuleAsync(ruleId);
        
        return Ok();
    }
}