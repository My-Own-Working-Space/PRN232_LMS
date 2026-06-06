namespace PRN232.LMS.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateAccessToken(int userId, string username, string role);
        public string GenerateRefreshToken();
    }
}
