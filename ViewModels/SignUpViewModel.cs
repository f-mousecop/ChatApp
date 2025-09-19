

using ChatApp.Commands;
using ChatApp.Services;
using ChatApp.Stores;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public string ConfirmPass { get; set; }
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;

        public ICommand NavigateSignUpCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public NavigationBarViewModel NavigationBarViewModel { get; }

        public SignUpViewModel(
            AccountStore accountStore,
            INavigationService<LoginViewModel> loginNavigationService)
        {
            _accountStore = accountStore;
            NavigateBackCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
        }
    }
}
