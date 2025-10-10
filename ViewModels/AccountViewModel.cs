using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
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
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateLogoutCommand { get; }
        public ICommand ChangeAvatarCommand { get; }
        public ICommand OpenAvatarPopup { get; }

        public AccountViewModel(
            AccountStore accountStore,
            INavigationService homeNavigationService,
            INavigationService chatNavigationService,
            INavigationService avatarViewNavigationService)
        {
            _accountStore = accountStore;
            _userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentDataFromStore();

            NavigateChatCommand = new NavigateCommand(chatNavigationService);

            //CloseAccountCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);

            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateLogoutCommand = new RelayCommand(
                _ => { _accountStore.Logout(); homeNavigationService.Navigate(); });

            ChangeAvatarCommand = new RelayCommand(async _ => await ExecuteChangeAvatarAsync(), _ => CurrentUserAccount?.Id > 0);
            OpenAvatarPopup = new NavigateCommand(avatarViewNavigationService);

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }


        private async Task ExecuteChangeAvatarAsync()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Choose an avatar image",
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = false
            };

            if (dlg.ShowDialog() != true) return;

            try
            {
                // 1) Save copy to app folder and resize
                var relPath = await AvatarService.SaveAvatarAsync(CurrentUserAccount.Id, dlg.FileName);

                // 2) Update DB
                await _userRepository.UpdateAvatarPathAsync(CurrentUserAccount.Id, relPath);

                // 3) Update in-memory model so UI refreshes
                CurrentUserAccount.AvatarUrl = relPath;
                LoadCurrentDataFromStore();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set avatar: {ex.Message}", "Avatar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SyncFieldsFromAccount(UserAccountModel acct)
        {
            AccountFields.Clear();
            if (acct == null) return;

            var name = !string.IsNullOrWhiteSpace(acct.FullName)
                ? acct.FullName
                : string.Join(" ", new[] { acct.FirstName, acct.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

            AccountFields.Add(new FieldItem("Name", name));
            AccountFields.Add(new FieldItem("Username", acct.Username));
            AccountFields.Add(new FieldItem("Role", acct.Role));
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
