

namespace ChatApp.Models.Auth
{
    /// <summary>
    /// Returns AuthResult
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string username, string password);
        Task<AuthResult> RegisterAsync(string username, string password); // For Sign Up Page later
    }
}
