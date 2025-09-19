using ChatApp.Commands;
using ChatApp.Services;
using ChatApp.Stores;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class NavigationBarViewModel
    {
        private readonly AccountStore _accountStore;
        public ICommand NavigateChatCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public NavigationBarViewModel(AccountStore accountStore,
            NavigationService<HomeViewModel> homeNavigationService,
            NavigationService<ChatViewModel> chatNavigationService,
            NavigationService<AccountViewModel> accountNavigationService,
            NavigationService<LoginViewModel> loginNavigationService)
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
        }
    }
}
