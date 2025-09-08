using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models.Auth
{
    /// <summary>
    /// Temporary Dummy Authentication service 
    /// Allows non-empty username & passwords to login successfully 
    /// </summary>
    public class DummyAuthServ : IAuthService
    {
        /// <summary>
        /// Login function
        /// </summary>
        /// <param name="username">username textbox</param>
        /// <param name="password">password passwordbox</param>
        /// <returns><see cref="AuthResult.Ok(User)"/> or <see cref="AuthResult.Fail(string)"/></returns>
        public Task<AuthResult> LoginAsync(string username, string password)
        {
            // Demo for production purposes: non-empty username & password success
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                return Task.FromResult(AuthResult.Ok(new User { Id = 1, Username = username }));
            }
            return Task.FromResult(AuthResult.Fail("Invalid username or password"));
        }

        /// <summary>
        /// Function to handle new user registration
        /// </summary>
        /// <param name="username">Grabs username</param>
        /// <param name="password">Grabs password</param>
        /// <returns>Auth result (User / success or error / fail</returns>
        public Task<AuthResult> RegisterAsync(string username, string password)
        {
            // Demo for production purposes: accept anything
            return Task.FromResult(AuthResult.Ok(new User { Id = 2, Username = username }));
        }
    }
}
