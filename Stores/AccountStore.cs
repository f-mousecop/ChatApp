using ChatApp.Models;
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
        private UserAccountModel _currentUserAccount;
        private IUserRepository _userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get => _currentUserAccount;
            set
            {
                _currentUserAccount = value;
            }
        }

        public string? Username => CurrentUserAccount?.Username;
        public bool IsLoggedIn => CurrentUserAccount != null;
    }
}
