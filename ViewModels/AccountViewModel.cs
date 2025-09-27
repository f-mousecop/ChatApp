using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        // Fields
        private readonly AccountStore _accountStore;
        private UserAccountModel _currentUserAccount;
        private IUserRepository _userRepository;

        public ObservableCollection<FieldItem> AccountFields { get; } = new();

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
            INavigationService homeNavigationService,
            INavigationService chatNavigationService)
        {
            _accountStore = accountStore;
            _userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentDataFromStore();

            NavigateChatCommand = new NavigateCommand(chatNavigationService);

            //CloseAccountCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);

            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateLogoutCommand = new LogoutCommand(_accountStore);

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

        ~AccountViewModel()
        {

        }

        private void SyncFieldsFromAccount(UserAccountModel acct)
        {
            AccountFields.Clear();
            if (acct == null) return;

            AccountFields.Add(new FieldItem("Name", acct.FullName));
            AccountFields.Add(new FieldItem("Username", acct.Username));
            AccountFields.Add(new FieldItem("Email", acct.Email));
            AccountFields.Add(new FieldItem("Mobile", acct.MobileNumber));
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
            SyncFieldsFromAccount(acct);
        }

        private void OnCurrentAccountChanged()
        {
            CurrentUserAccount = _accountStore.CurrentUserAccount;
            SyncFieldsFromAccount(CurrentUserAccount);

            //if (_currentUserAccount != null)
            //{
            //    CurrentUserAccount.DisplayName = string.Empty;
            //    CurrentUserAccount.Email = string.Empty;
            //    OnPropertyChanged(nameof(CurrentUserAccount));
            //    return;
            //}
        }


        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
            base.Dispose();
        }
    }
}
