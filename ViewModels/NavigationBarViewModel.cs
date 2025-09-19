using ChatApp.Commands;
using ChatApp.Services;
using ChatApp.Stores;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        private readonly AccountStore _accountStore;
        public ICommand NavigateChatCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateSignUpCommand { get; }

        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public NavigationBarViewModel(
            AccountStore accountStore,
            INavigationService<HomeViewModel> homeNavigationService,
            INavigationService<ChatViewModel> chatNavigationService,
            INavigationService<AccountViewModel> accountNavigationService,
            INavigationService<LoginViewModel> loginNavigationService,
            INavigationService<SignUpViewModel> signUpNavigationService)
        {
            _accountStore = accountStore;

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>
                (homeNavigationService);
            NavigateChatCommand = new NavigateCommand<ChatViewModel>
                (chatNavigationService);
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>
                (accountNavigationService);
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>
                (loginNavigationService);
            NavigateSignUpCommand = new NavigateCommand<SignUpViewModel>
                (signUpNavigationService);
        }
    }
}
