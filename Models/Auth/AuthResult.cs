
namespace ChatApp.Models.Auth
{
    public class AuthResult
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public UserModel? User { get; init; }

        /// <summary>
        /// Attempt to login
        /// </summary>
        /// <param name="u">User</param>
        /// <returns><see cref="User"/> and <see cref="Success = true"/></returns>
        public static AuthResult Ok(UserModel u) => new() { User = u, Success = true };

        /// <summary>
        /// If login fails
        /// </summary>
        /// <param name="e">Error</param>
        /// <returns><see cref="Error"/> and <see cref="Success = false"/></returns>
        public static AuthResult Fail(string e) => new() { Error = e, Success = false };
    }
}
