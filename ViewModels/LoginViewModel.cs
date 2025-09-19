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
        private string _errorMessage;

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
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
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
            NavigationBarViewModel navigationBarViewModel,
            INavigationService<AccountViewModel> accountNavigationService,
            INavigationService<SignUpViewModel> signUpNavigationService)
        {
            _accountStore = accountStore;
            NavigationBarViewModel = navigationBarViewModel;
            _userRepository = new UserRepository();

            LoginCommand = new RelayCommand(async _ => await ExecuteLoginCommand(), _ => CanExecuteLoginCommand());

            //NavigateChatCommand = new NavigateCommand<ChatViewModel>(chatNavigationService);

            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(accountNavigationService);

            NavigateSignUpCommand = new NavigateCommand<SignUpViewModel>(signUpNavigationService);

            RecoverPasswordCommand = new RelayCommand(_ => ExecuteRecoverPassCommand("", ""));
            ShowPasswordCommand = new RelayCommand(_ => { /* TODO */ });
            QuitCommand = new RelayCommand(_ => System.Windows.Application.Current.Shutdown());
        }

        public bool CanExecuteLoginCommand()
        {
            bool validData;
            if (string.IsNullOrEmpty(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
            {
                validData = false;
            }
            else validData = true;
            return validData;
        }


        private async Task ExecuteLoginCommand()
        {
            try
            {
                var isValidUser = await _userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
                if (isValidUser)
                {
                    var user = _userRepository.GetByUsername(Username);
                    _accountStore.CurrentUserAccount = new UserAccountModel
                    {
                        Username = user?.Username ?? Username,
                        Email = user?.Email ?? "",
                        DisplayName = $"Welcome {Username}",
                    };

                    //Thread.CurrentPrincipal = new GenericPrincipal(
                    //    new GenericIdentity(Username), null);
                    //NavigateChatCommand.Execute(LoginCommand);
                    NavigateAccountCommand.Execute(null);

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
