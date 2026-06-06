using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Interfaces
{
    public interface IAuthService
    {
        public LoginModel Login(string username, string password);
        public LoginModel RefreshToken(string token);
        public void Register(string username, string password, string email, string fullName, DateTime dateOfBirth);
    }
}
