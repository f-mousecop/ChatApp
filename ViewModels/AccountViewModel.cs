using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChatApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        // Fields
        private readonly AccountStore _accountStore;
        private UserAccountModel _currentUserAccount;
        private IUserRepository _userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get => _currentUserAccount;
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ICommand NavigateChatCommand { get; }
        public ICommand CloseAccountCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        public AccountViewModel(
            AccountStore accountStore,
            INavigationService<HomeViewModel> homeNavigationService,
            INavigationService<ChatViewModel> chatNavigationService)
        {
            _accountStore = accountStore;
            _userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentDataFromStore();

            NavigateChatCommand = new NavigateCommand<ChatViewModel>(chatNavigationService);

            //CloseAccountCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(homeNavigationService);
        }

        private void LoadCurrentDataFromStore()
        {
            var acct = _accountStore.CurrentUserAccount;
            if (acct == null)
            {
                MessageBox.Show("Session expired. Please log in again.");
                return;
            }
            CurrentUserAccount = acct;

            //var username = Thread.CurrentPrincipal?.Identity?.Name;

            //if (string.IsNullOrWhiteSpace(username))
            //{
            //    MessageBox.Show("Session expired. Please log in again.");
            //    CloseAccountCommand.Execute(navigationStore);
            //    return;
            //}

            //var user = _userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            //if (user != null)
            //{

            //    _accountStore.CurrentUserAccount.Username = user.Username;
            //    CurrentUserAccount.DisplayName = $"Welcome {user.Username} ;)";
            //    CurrentUserAccount.Email = user.Email;
            //    CurrentUserAccount.CurrentTime = DateTimeOffset.UtcNow.ToLocalTime().ToString();
            //    CurrentUserAccount.ProfilePicture = null;

            //}
            //else
            //{
            //    CurrentUserAccount.DisplayName = "Invalid user, not logged in";
            //}

        }
    }
}
