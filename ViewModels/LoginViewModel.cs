using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using ChatApp.Utils;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatApp.ViewModels
{

    public class LoginViewModel : BaseViewModel
    {
        private string _username = string.Empty;
        private SecureString _password;
        private string _errorMessage = string.Empty;
        public bool ShowErrorMessage => !string.IsNullOrEmpty(_errorMessage);

        private readonly AccountStore _accountStore;
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

        public SecureString Password
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
                    OnPropertyChanged(nameof(ShowErrorMessage));
            }
        }


        // Commands
        public ICommand ApplyThemeCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand NavigateChatCommand { get; }
        public ICommand NavigateSignUpCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand QuitCommand { get; }
        public NavigationBarViewModel NavigationBarViewModel { get; }


        // View decides how to navigate accordingly
        //public event Action? LoginSucceeded;

        public LoginViewModel(

            AccountStore accountStore,
            INavigationService accountNavigationService,
            INavigationService signUpNavigationService)
        {
            _accountStore = accountStore;
            _userRepository = new UserRepository();

            LoginCommand = new RelayCommand(async _ => await ExecuteLoginCommand(), _ => CanExecuteLoginCommand());

            //NavigateChatCommand = new NavigateCommand<ChatViewModel>(chatNavigationService);

            NavigateAccountCommand = new NavigateCommand(accountNavigationService);

            NavigateSignUpCommand = new NavigateCommand(signUpNavigationService);

            RecoverPasswordCommand = new RelayCommand(_ => ExecuteRecoverPassCommand("", ""));
            ShowPasswordCommand = new RelayCommand(_ => { /* TODO */ });
            QuitCommand = new RelayCommand(_ => System.Windows.Application.Current.Shutdown());
        }

        public bool CanExecuteLoginCommand()
        {
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3) return false;
            if (Password == null || Password.Length < 3) return false;

            return true;
        }


        private async Task ExecuteLoginCommand()
        {
            ErrorMessage = string.Empty;

            try
            {
                var isValidUser = await _userRepository.AuthenticateUserAsync(Username, Password);
                if (!isValidUser)
                {
                    ErrorMessage = "Invalid username or password.";
                    return;
                }

                var user = await _userRepository.GetByUsernameAsync(Username);

                string fullName = string.Join(" ",
                    new[] { user?.FirstName, user?.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

                _accountStore.CurrentUserAccount = new UserAccountModel
                {
                    Id = int.TryParse(user?.Id, out var id) ? id : 0,
                    Username = user?.Username ?? Username,
                    Email = user?.Email ?? "",
                    FirstName = user?.FirstName ?? "",
                    LastName = user?.LastName ?? "",
                    FullName = string.IsNullOrWhiteSpace(fullName) ? "" : fullName,
                    MobileNumber = user?.MobileNumber ?? "",
                    DisplayName = string.IsNullOrWhiteSpace(user?.FirstName)
                                    ? $"Welcome, {Username}"
                                    : $"Welcome, {user!.FirstName}",
                };

                NavigateAccountCommand.Execute(null);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorMessage = e.Message;
            }


        }

        //private Action<object> ExecuteShowPasswordCommand()
        //{
        //    throw new NotImplementedException();
        //}

        //private void ApplyTheme(object obj)
        //{
        //    var gradient = new LinearGradientBrush
        //    {
        //        StartPoint = new Point(0, 0),
        //        EndPoint = new Point(1, 1),
        //        GradientStops = {
        //            new GradientStop((Color)ColorConverter.ConvertFromString("#0f172a"), 0.0),
        //            new GradientStop((Color)ColorConverter.ConvertFromString("#6b21a8"), 1.0),
        //        }
        //    };

        //    ThemeService.SetWindowTheme(gradient);
        //}



        private void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
