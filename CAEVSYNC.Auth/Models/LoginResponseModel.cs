namespace CAEVSYNC.Auth.Models;

public class LoginResponseModel
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}