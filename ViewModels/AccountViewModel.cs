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
        public ICommand NavigateLogoutCommand { get; }

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
            NavigateLogoutCommand = new LogoutCommand(_accountStore);

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

        ~AccountViewModel()
        {

        }

        private void OnCurrentAccountChanged()
        {
            if (_currentUserAccount != null)
            {
                CurrentUserAccount.DisplayName = string.Empty;
                CurrentUserAccount.Email = string.Empty;
                OnPropertyChanged(nameof(CurrentUserAccount));
                return;
            }
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
        }

        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
            base.Dispose();
        }
    }
}
