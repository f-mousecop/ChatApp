using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models.Auth
{
    public class DummyAuthServ : IAuthService
    {
        public Task<AuthResult> LoginAsync(string username, string password)
        {
            // Demo for production purposes: non-empty username & password success
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                return Task.FromResult(AuthResult.Ok(new User { Id = 1, Username = username }));
            }
            return Task.FromResult(AuthResult.Fail("Invalid Login"));
        }

        public Task<AuthResult> RegisterAsync(string username, string password)
        {
            // Demo for production purposes: accept anything
            return Task.FromResult(AuthResult.Ok(new User { Id = 2, Username = username }));
        }
    }
}
