

using System.Security;

namespace ChatApp.Models.Auth
{
    /// <summary>
    /// Returns AuthResult
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string username, SecureString password, CancellationToken ct = default);
        Task<AuthResult> RegisterAsync(UserModel newUser, CancellationToken ct = default); // For Sign Up Page later
        Task<AuthResult> ChangePasswordAsync(int userId, SecureString oldPassword, SecureString newPassword, CancellationToken ct = default);
    }
}
