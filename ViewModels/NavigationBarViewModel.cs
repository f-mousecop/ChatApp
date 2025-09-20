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
        public ICommand NavigateLogoutCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateSignUpCommand { get; }

        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public bool IsNotLoggedIn => _accountStore.IsNotLoggedIn;
        public NavigationBarViewModel(
            AccountStore accountStore,
            INavigationService homeNavigationService,
            INavigationService chatNavigationService,
            INavigationService accountNavigationService,
            INavigationService loginNavigationService,
            INavigationService signUpNavigationService)
        {
            _accountStore = accountStore;

            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateChatCommand = new NavigateCommand(chatNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            NavigateLogoutCommand = new LogoutCommand(_accountStore);
            NavigateSignUpCommand = new NavigateCommand(signUpNavigationService);
            NavigateLoginCommand = new NavigateCommand(loginNavigationService);

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;

            base.Dispose();
        }

    }
}
