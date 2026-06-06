namespace PRN232.LMS.Services.Models;

public class LoginModel
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public int ExpiresIn { get; set; }
}