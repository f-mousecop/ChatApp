using ChatApp.Models;
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
    public class AvatarViewModel : BaseViewModel
    {
        private readonly AccountStore _accountStore;
        private readonly INavigationService _closeModalService;
        private readonly INavigationService _navigateAccountService;


        private UserAccountModel _currentUserAccount;
        public UserAccountModel CurrentUserAccount
        {
            get => _currentUserAccount;
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ICommand CloseAvatarModalCommand { get; }

        public AvatarViewModel(
            AccountStore accountStore,
            INavigationService closeModalService,
            INavigationService navigateAccountService)
        {
            _accountStore = accountStore;
            _closeModalService = closeModalService;
            _navigateAccountService = navigateAccountService;
            CurrentUserAccount = new UserAccountModel();

            LoadCurrentDataFromStore();

            CloseAvatarModalCommand = new RelayCommand(
                _ => { _closeModalService.Navigate(); navigateAccountService.Navigate(); },
                _ => _accountStore.IsLoggedIn);

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

        private void LoadCurrentDataFromStore()
        {
            var acct = _accountStore.CurrentUserAccount;
            if (acct == null)
            {
                MessageBox.Show("No user account data to display.");
                return;
            }
            CurrentUserAccount = acct;
        }

        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(_accountStore.CurrentUserAccount));
        }

        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
            base.Dispose();
        }
    }
}
