using ChatApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models.Auth
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResult> LoginAsync(string username, SecureString password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(username) || password is null || password.Length < 3)
            {
                return AuthResult.Fail("Invalid username or password");
            }

            var auth = await _userRepository.GetAuthByUsernameAsync(username);
            if (auth is null) return AuthResult.Fail("Invalid username or password");

            var plain = password.ToUnsecureString();
            var (_, hash) = auth.Value;
            if (!BCrypt.Net.BCrypt.Verify(plain, hash))
                return AuthResult.Fail("Invalid username or password");

            // Load profile
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null) return AuthResult.Fail("Failed to load user profile");
            plain = string.Empty;
            return AuthResult.Ok(user);
        }

        public Task<AuthResult> RegisterAsync(UserModel newUser, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        public Task<AuthResult> ChangePasswordAsync(int userId, SecureString oldPassword, SecureString newPassword, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
