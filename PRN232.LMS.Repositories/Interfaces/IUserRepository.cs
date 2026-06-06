using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserByUsername(string username);
        public User GetUserById(int userId);
        public void AddUser(User user);
        public void AddRefreshToken(RefreshToken token);
        public RefreshToken GetRefreshToken(string token);
        public void UpdateRefreshToken(RefreshToken token);
    }
}