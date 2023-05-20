using CAEVSYNC.Auth.Models;
using CAEVSYNC.Auth.Services;
using CAEVSYNC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAEVSYNC.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly CaevsyncDbContext _dbContext;
    private readonly JwtTokenService _tokenService;
    
    public AuthController(
        UserManager<IdentityUser> userManager,
        CaevsyncDbContext dbContext,
        JwtTokenService tokenService)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _tokenService = tokenService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationModel registrationModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var identityResult = await _userManager.CreateAsync(
            new IdentityUser
            {
                UserName = registrationModel.Username, 
                Email = registrationModel.Email
            },
            registrationModel.Password);
        
        if (identityResult.Succeeded)
        {
            registrationModel.Password = "";
            return CreatedAtAction(
                nameof(Register), 
                new {email = registrationModel.Email}, 
                registrationModel);
        }
        
        foreach (var error in identityResult.Errors) 
            ModelState.AddModelError(error.Code, error.Description);
        
        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> LoginAsync([FromBody] LoginModel loginModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var managedUser = await _userManager.FindByEmailAsync(loginModel.Email);
        if (managedUser == null)
            return BadRequest("User with such email doesn't exist");
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, loginModel.Password);
        if (!isPasswordValid)
            return BadRequest("Wrong password");
        
        var userInDb = _dbContext.Users.FirstOrDefault(u => u.Email == loginModel.Email);
        if (userInDb is null)
            return Unauthorized();
        var accessToken = _tokenService.CreateToken(userInDb);
        await _dbContext.SaveChangesAsync();
        return Ok(new LoginResponseModel
        {
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
        });
    }
    
}