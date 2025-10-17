using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Models.Auth;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using ChatApp.Utils;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatApp.ViewModels
{

    public class LoginViewModel : BaseViewModel
    {
        private string _username = string.Empty;
        private SecureString _password;
        private string _errorMessage = string.Empty;

        public SnackbarManager Snackbar { get; }

        public bool ShowErrorMessage => !string.IsNullOrEmpty(_errorMessage);
        public bool IsAdmin => _accountStore.IsAdmin;

        private bool _showPassword;
        public bool ShowPassword
        {
            get => _showPassword;
            set => SetProperty(ref _showPassword, value);
        }

        private readonly AccountStore _accountStore;
        private readonly INavigationService _closeModalService;
        private readonly INavigationService _userDestination;
        private readonly INavigationService _adminDestination;
        private readonly IUserRepository _userRepository;

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

        //private SnackbarMessageQueue _messageQueue;
        //public SnackbarMessageQueue MessageQueue
        //{
        //    get => _messageQueue;
        //    set => SetProperty(ref _messageQueue, value);
        //}


        // Commands
        public ICommand ApplyThemeCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand NavigateSignUpCommand { get; }
        public ICommand NavigateToForgotPassCommand { get; }
        public ICommand QuitCommand { get; }
        public ICommand CloseModalCommand { get; }
        public NavigationBarViewModel NavigationBarViewModel { get; }


        // View decides how to navigate accordingly
        //public event Action? LoginSucceeded;

        public LoginViewModel(

            AccountStore accountStore,
            INavigationService closeModalService,
            INavigationService userDestination,
            INavigationService adminDestination,
            INavigationService signUpNavigationService)
        {
            _accountStore = accountStore;
            _closeModalService = closeModalService;
            _userDestination = userDestination;
            _adminDestination = adminDestination;
            _userRepository = new UserRepository();

            var queue = new SnackbarMessageQueue();

            Snackbar = new SnackbarManager(queue);

            LoginCommand = new RelayCommand(async _ => await ExecuteLoginCommand(), _ => CanExecuteLoginCommand());
            CloseModalCommand = new RelayCommand(_ => _closeModalService.Navigate());
            NavigateSignUpCommand = new NavigateCommand(signUpNavigationService);

            NavigateToForgotPassCommand = new RelayCommand(_ => ExecuteRecoverPassCommand("", ""));
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
                    SnackbarManager.Instance.Enqueue(ErrorMessage, SnackbarManager.MessageToSnackLevel.Error,
                        duration: TimeSpan.FromSeconds(10));
                    return;
                }



                var user = await _userRepository.GetByUsernameAsync(Username);

                string fullName = string.Join(" ",
                    new[] { user?.FirstName, user?.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

                _accountStore.CurrentUserAccount = new UserAccountModel
                {
                    Id = Convert.ToInt32(user?.Id),
                    Username = user?.Username ?? Username,
                    Email = user?.Email ?? "",
                    FirstName = user?.FirstName ?? "",
                    LastName = user?.LastName ?? "",
                    Role = user?.Role ?? "",
                    FullName = string.IsNullOrWhiteSpace(fullName) ? "" : fullName,
                    MobileNumber = user?.MobileNumber ?? "",
                    DisplayName = string.IsNullOrWhiteSpace(user?.FirstName)
                                    ? $"Welcome, {Username}"
                                    : $"Welcome, {user!.FirstName}",
                    AvatarUrl = string.IsNullOrWhiteSpace(user?.AvatarUrl) ? null : user!.AvatarUrl,
                };

                OnPropertyChanged(nameof(IsAdmin));

                SnackbarManager.Instance.Enqueue($"Login Success, welcome, {Username}", SnackbarManager.MessageToSnackLevel.Success,
                    duration: TimeSpan.FromSeconds(5));

                _closeModalService.Navigate();

                if (_accountStore.IsAdmin)
                {
                    _adminDestination.Navigate();
                }
                else
                {
                    _userDestination.Navigate();
                }


            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                SnackbarManager.Instance.Enqueue(e.Message, SnackbarManager.MessageToSnackLevel.Error,
                        duration: TimeSpan.FromSeconds(10));
                ErrorMessage = e.Message;
            }


        }


        /* THIS IS WHERE I WANT TO TEST THE NEW SNACKBAR MANAGER */
        private void ExecuteRecoverPassCommand(string username, string email)
        {
            SnackbarManager.Instance.Enqueue("Testing snackbar", SnackbarManager.MessageToSnackLevel.Success,
                        duration: TimeSpan.FromSeconds(10), true);
            return;
        }
    }
}
