using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using System;

namespace PRN232.LMS.Services
{
    public class AuthService(IUserRepository _userRepository, IJwtService _jwtService) : IAuthService
    {
        public LoginModel Login(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var accessToken = _jwtService.GenerateAccessToken(user.UserId, user.Username, user.Role);
            var refreshTokenStr = _jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshTokenStr,
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            _userRepository.AddRefreshToken(refreshToken);

            return new LoginModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshTokenStr,
                ExpiresIn = 3600
            };
        }

        public LoginModel RefreshToken(string token)
        {
            var existingToken = _userRepository.GetRefreshToken(token);

            if (existingToken == null || existingToken.Expires < DateTime.Now || existingToken.Revoked != null)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var user = _userRepository.GetUserById(existingToken.UserId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            // Revoke current token
            existingToken.Revoked = DateTime.Now;
            _userRepository.UpdateRefreshToken(existingToken);

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateAccessToken(user.UserId, user.Username, user.Role);
            var newRefreshTokenStr = _jwtService.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = newRefreshTokenStr,
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            _userRepository.AddRefreshToken(newRefreshToken);

            return new LoginModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenStr,
                ExpiresIn = 3600
            };
        }

        public void Register(string username, string password, string email, string fullName, DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }
    }
}