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

namespace ChatApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
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

        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public bool IsNotLoggedIn => _accountStore.IsNotLoggedIn;

        // Commands
        public ICommand NavigateChatCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateAccountCommand { get; }

        public HomeViewModel(
            AccountStore accountStore,
            INavigationService loginNavigationService,
            INavigationService chatNavigationService,
            INavigationService accountNavigationService)
        {
            _accountStore = accountStore;
            _userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentDataFromStore();

            NavigateLoginCommand = new NavigateCommand(loginNavigationService);
            NavigateChatCommand = new NavigateCommand(chatNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);

        }


        private void LoadCurrentDataFromStore()
        {
            var acct = _accountStore.CurrentUserAccount;
            if (acct != null)
            {
                _currentUserAccount = acct;
            }
            return;
        }
    }
}
