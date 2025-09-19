using ChatApp.Services;
using ChatApp.Stores;
using ChatApp.ViewModels;
using System.Windows;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly AccountStore _accountStore;
        private readonly NavigationStore _navigationStore;
        private readonly NavigationBarViewModel _navigationBarViewModel;

        public App()
        {
            _accountStore = new AccountStore();
            _navigationStore = new NavigationStore();
            _navigationBarViewModel = new NavigationBarViewModel(
                _accountStore,
                CreateHomeNavigationService(),
                CreateChatNavigationService(),
                CreateAccountNavigationService(),
                CreateLoginNavigationService(),
                CreateSignUpNavigationService());
        }


        protected override void OnStartup(StartupEventArgs e)
        {

            INavigationService<HomeViewModel> homeNavigationService = CreateHomeNavigationService();
            homeNavigationService.Navigate();
            MainWindow = new MainWindow()
            {
                DataContext = new WindowViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService<SignUpViewModel> CreateSignUpNavigationService()
        {
            return new NavigationService<SignUpViewModel>
                (_navigationStore,
                () => new SignUpViewModel(_accountStore, CreateLoginNavigationService()));
        }

        private INavigationService<HomeViewModel> CreateHomeNavigationService()
        {
            return new LayoutNavigationService<HomeViewModel>
                (_navigationStore,
                () => new HomeViewModel
                    (CreateLoginNavigationService()),
                    _navigationBarViewModel);
        }
        private INavigationService<AccountViewModel> CreateAccountNavigationService()
        {
            return new LayoutNavigationService<AccountViewModel>
                (_navigationStore,
                () => new AccountViewModel
                    (
                    _accountStore,
                    CreateHomeNavigationService(),
                    CreateChatNavigationService()),
                _navigationBarViewModel);
        }

        private INavigationService<ChatViewModel> CreateChatNavigationService()
        {
            return new NavigationService<ChatViewModel>
                (_navigationStore,
                () => new ChatViewModel
                    (_accountStore,
                    CreateAccountNavigationService()));
        }
        private INavigationService<LoginViewModel> CreateLoginNavigationService()
        {
            return new NavigationService<LoginViewModel>
                (_navigationStore,
                () => new LoginViewModel
                    (_accountStore,
                    CreateAccountNavigationService(), CreateSignUpNavigationService()));
        }
    }

}
