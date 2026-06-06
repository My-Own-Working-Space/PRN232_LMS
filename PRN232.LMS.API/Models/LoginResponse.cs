namespace PRN232.LMS.API.Models;

public class LoginResponse
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public int ExpiresIn { get; set; }
}