using ChatApp.Models;
using ChatApp.Models.Auth;
using ChatApp.Repositories;
using ChatApp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ChatApp.ViewModels
{
    // 1. Initialize _auth via constructor injection (add a constructor overload).
    // 2. Initialize _username and _password to empty strings.
    // 3. Initialize ShowPasswordCommand to a default RelayCommand (even if not implemented yet).

    public class LoginViewModel : BaseViewModel
    {
        //private readonly IAuthService? _auth;
        private string _username = string.Empty;
        private string _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository _userRepository;

        public string Username
        {
            get { return _username; }
            set
            {
                if (SetProperty(ref _username, value))
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (SetProperty(ref _errorMessage, value))
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }


        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                if (SetProperty(ref _isViewVisible, value))
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        // Commands
        public ICommand ApplyThemeCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand QuitCommand { get; }


        // View decides how to navigate accordingly
        //public event Action? LoginSucceeded;

        public LoginViewModel()
        {
            //ApplyThemeCommand = new RelayCommand(ApplyTheme);
            //IAuthService _auth;
            //DummyAuthServ dummyAuthServ = new();
            _userRepository = new UserRepository();

            //LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());
            LoginCommand = new RelayCommand(_ => ExecuteLoginCommand(), _ => CanExecuteLoginCommand());
            RecoverPasswordCommand = new RelayCommand(_ => ExecuteRecoverPassCommand("", ""));
            ShowPasswordCommand = new RelayCommand(_ => { /* TODO */ });
            QuitCommand = new RelayCommand(_ => System.Windows.Application.Current.Shutdown());
        }


        public bool CanExecuteLoginCommand()
        {
            bool validData = false;
            if ((!string.IsNullOrEmpty(Password)) &&
                (!string.IsNullOrEmpty(Username)) &&
                (Password.Length >= 3) && (Username.Length >= 3))
            {
                validData = true;
            }
            return validData;
        }

        private void ExecuteLoginCommand()
        {
            try
            {
                var isValidUser = _userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
                if (isValidUser)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity(Username), null);
                    IsViewVisible = false;
                }
                else
                {
                    ErrorMessage = "* Invalid Username or Password";
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorMessage = e.Message;
                Debug.WriteLine(e.Message);
            }
        }

        private Action<object> ExecuteShowPasswordCommand()
        {
            throw new NotImplementedException();
        }

        private void ApplyTheme(object obj)
        {
            var gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = {
                    new GradientStop((Color)ColorConverter.ConvertFromString("#0f172a"), 0.0),
                    new GradientStop((Color)ColorConverter.ConvertFromString("#6b21a8"), 1.0),
                }
            };

            ThemeService.SetWindowTheme(gradient);
        }

        //private bool CanLogin()
        //{
        //    bool validData;
        //    if (string.IsNullOrEmpty(Username) || Username.Length < 3 ||
        //        Password == null || Password.Length < 3)
        //        validData = false;
        //    else validData = true;
        //    return validData;
        //}



        //private async Task LoginAsync()
        //{
        //    var ok = await _auth.LoginAsync(Username, Password);
        //    if (ok.Success) LoginSucceeded?.Invoke();
        //    else MessageBox.Show(
        //        ok.Error ?? "Login Failed", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //}

        private void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
