using ChatApp.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public UserViewModel User { get; } = new();

        private readonly IAuthService _auth;

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
            //{
            //    if (SetProperty(ref _password, value))
            //        ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            //}
        }

        public ICommand LoginCommand { get; }
        public ICommand QuitCommand { get; }


        // View decides how to navigate accordingly
        public event Action? LoginSucceeded;

        public LoginViewModel(IAuthService auth)
        {
            _auth = auth;

            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());
            QuitCommand = new RelayCommand(_ => System.Windows.Application.Current.Shutdown());

            User.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(UserViewModel.Username) || e.PropertyName == nameof(Password))
                {
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            };
        }

        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(User.Username) &&
            !string.IsNullOrWhiteSpace(Password);

        private async Task LoginAsync()
        {
            var ok = await _auth.LoginAsync(User.Username, Password);
            if (ok.Success) LoginSucceeded?.Invoke();
            else MessageBox.Show(
                "Invalid username or password.",
                "Login Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //try
            //{
            //    LoginSucceeded?.Invoke();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show($"Exception: {e}", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    throw;
            //}
        }
    }
}
