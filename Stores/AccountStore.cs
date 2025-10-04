using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Stores
{
    public class AccountStore
    {
        // Fields
        private UserAccountModel? _currentUserAccount;
        private readonly IUserRepository? _userRepository;

        public event Action? CurrentAccountChanged;
        public UserAccountModel? CurrentUserAccount
        {
            get => _currentUserAccount;
            set
            {
                _currentUserAccount = value;
                CurrentAccountChanged?.Invoke();
            }
        }

        public string? Username => CurrentUserAccount?.Username;
        public bool IsLoggedIn => CurrentUserAccount != null;
        public bool IsNotLoggedIn => CurrentUserAccount == null;

        // Check if user has the role "Admin"
        public bool IsAdmin => CurrentUserAccount != null && string.Equals(CurrentUserAccount.Role, "Admin", StringComparison.OrdinalIgnoreCase);


        public void Logout()
        {
            CurrentUserAccount = null;
        }
    }
}
