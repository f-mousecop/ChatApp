using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models.Auth
{
    /// <summary>
    /// TODO: Implement Firebase DB auth service for login/storage
    /// </summary>
    //public class FirebaseAuthService : IAuthService
    //{
    //    public Task<AuthResult> LoginAsync(string username, string password)
    //    {
    //        // Demo for production purposes: non-empty username & password success
    //        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
    //        {
    //            return Task.FromResult(AuthResult.Ok(new User { Id = 1, Username = username }));
    //        }
    //        return Task.FromResult(AuthResult.Fail("Invalid username or password."));
    //    }

    //    public Task<AuthResult> RegisterAsync(string username, string password)
    //    {
    //        // Demo for production purposes: accept anything
    //        return Task.FromResult(AuthResult.Ok(new User { Id = 2, Username = username }));
    //    }
    //}
}
