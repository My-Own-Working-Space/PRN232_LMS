using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories
{
    public class UserRepository(LMSDatabaseContext _context) : IUserRepository
    {
        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddRefreshToken(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            _context.SaveChanges();
        }

        public RefreshToken GetRefreshToken(string token)
        {
            return _context.RefreshTokens.FirstOrDefault(t => t.Token == token);
        }

        public void UpdateRefreshToken(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            _context.SaveChanges();
        }
    }
}