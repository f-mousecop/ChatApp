

using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using ChatApp.Utils;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;
        private readonly IUserRepository _userRepository;

        private string _firstName = "";
        public string Firstname
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                    ((RelayCommand)SubmitCommand).RaiseCanExecuteChanged();
            }
        }

        private string _lastName = "";
        public string Lastname
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                    ((RelayCommand)SubmitCommand).RaiseCanExecuteChanged();
            }
        }

        private string _username = "";
        public string Username
        {
            get => _username;
            set
            {
                if (SetProperty(ref _username, value))
                    ((RelayCommand)SubmitCommand).RaiseCanExecuteChanged();
            }
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                    ((RelayCommand)SubmitCommand).RaiseCanExecuteChanged();
            }
        }

        private string _mobileNumber = "";
        public string MobileNumber
        {
            get => _mobileNumber;
            set
            {
                if (SetProperty(ref _mobileNumber, value))
                    ((RelayCommand)SubmitCommand).RaiseCanExecuteChanged();
            }
        }

        public SecureString Password { get; set; }
        public SecureString ConfirmPass { get; set; }

        private string _error = "";
        public string Error
        {
            get => _error;
            set
            {
                SetProperty(ref _error, value);
            }
        }


        public ICommand SubmitCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public NavigationBarViewModel NavigationBarViewModel { get; }

        public SignUpViewModel(
            AccountStore accountStore,
            INavigationService loginNavigationService,
            INavigationService chatNavigationService)
        {
            _accountStore = accountStore;
            _userRepository = new UserRepository();

            NavigateBackCommand = new NavigateCommand(loginNavigationService);
            SubmitCommand = new RelayCommand(async _ => await SubmitAsync(), _ => CanSubmit());
        }

        private bool CanSubmit()
        {
            if (string.IsNullOrWhiteSpace(Firstname)) return false;
            if (string.IsNullOrWhiteSpace(Lastname)) return false;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3) return false;
            if (string.IsNullOrWhiteSpace(Email)) return false;
            if (Password == null || Password.Length < 6) return false;
            if (ConfirmPass == null || ConfirmPass.Length < 6) return false;

            // Compare passwords and check if match
            var pass = Password.ToUnsecureString();
            var confPass = ConfirmPass.ToUnsecureString();
            var match = pass == confPass;
            pass = confPass = string.Empty;

            return match;
        }

        //private bool IsEmail(string email) =>
        //    // Regex expression to ensure correct email
        //    Regex.IsMatch(email, @"^[@\s]+@[^@\s]+\.[^@\s]+$");

        private async Task SubmitAsync()
        {
            Error = "";
            try
            {
                var plain = Password.ToUnsecureString();
                try
                {
                    var user = new UserModel
                    {
                        FirstName = Firstname,
                        LastName = Lastname,
                        Username = Username,
                        Email = Email,
                        MobileNumber = MobileNumber,
                        Password = plain,
                        IsEmailVerified = false
                    };

                    // create new user
                    await _userRepository.AddAsync(user);

                    MessageBox.Show("Account created! Please log in.", "Sign Up", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    NavigateBackCommand.Execute(null);
                }

                finally { plain = string.Empty; }
            }
            catch (InvalidOperationException ex)
            {
                Error = ex.Message;
            }
            catch (Exception ex)
            {
                Error = "Sign up failed. Please try again.";
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
