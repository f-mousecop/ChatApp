using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models.Auth
{
    public class AuthResult
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public User? User { get; init; }

        public static AuthResult Ok(User u) => new() { User = u, Success = true };
        public static AuthResult Fail(string e) => new() { Error = e, Success = false };
    }
}
