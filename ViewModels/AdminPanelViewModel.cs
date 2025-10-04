using ChatApp.Models;
using ChatApp.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class AdminPanelViewModel : BaseViewModel
    {
        private readonly AccountStore _accountStore;
        private readonly IUserRepository _userRepository;

        private string _errorMessage = string.Empty;
        public bool ShowErrorMessage => !string.IsNullOrEmpty(_errorMessage);

        public ObservableCollection<UserModel> Users { get; } = [];

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (SetProperty(ref _errorMessage, value))
                    OnPropertyChanged(nameof(ShowErrorMessage));
            }
        }

        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand DeleteSelectedCommand { get; }

        public AdminPanelViewModel(AccountStore accountStore, IUserRepository userRepository)
        {
            _accountStore = accountStore;
            _userRepository = userRepository;
            RefreshCommand = new RelayCommand(async _ => await LoadAsync());
            DeleteSelectedCommand = new RelayCommand(async _ => await DeleteSelectedAsync(), _ => SelectedUser != null);
            _ = LoadAsync();
        }

        private async Task DeleteSelectedAsync()
        {
            if (SelectedUser is null) return;
            var rows = await _userRepository.RemoveAsync(SelectedUser.Id);

            if (rows > 0)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
            }
        }

        private async Task LoadAsync()
        {
            ErrorMessage = string.Empty;
            try
            {
                var all = await _userRepository.GetByAllAsync();
                Users.Clear();
                foreach (var user in all)
                {
                    Users.Add(user);
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
