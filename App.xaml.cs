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
        private readonly AccountStore _accountStore = new();
        private readonly NavigationStore _navigationStore = new();

        private NavigationBarViewModel _navigationBarViewModel;
        private NavigationBarViewModel GetNavBar() => _navigationBarViewModel;

        public App()
        {
            //_accountStore = new AccountStore();
            //_navigationStore = new NavigationStore();
            //_navigationBarViewModel = new NavigationBarViewModel(
            //    _accountStore,
            //    CreateHomeNavigationService(),
            //    CreateChatNavigationService(),
            //    CreateAccountNavigationService(),
            //    CreateLoginNavigationService(),
            //    CreateSignUpNavigationService());
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            // Build services using GetNavBar (no direct nav-bar dependency yet
            var homeSvc = CreateHomeNavigationService();
            var chatSvc = CreateChatNavigationService();
            var accountSvc = CreateAccountNavigationService();
            var loginSvc = CreateLoginNavigationService();
            var signUpSvc = CreateSignUpNavigationService();

            // Creating one nav bar VM using those services
            _navigationBarViewModel = new NavigationBarViewModel(
                _accountStore, homeSvc, chatSvc, accountSvc, loginSvc, signUpSvc);

            // Nav to home
            homeSvc.Navigate();

            //INavigationService<HomeViewModel> homeNavigationService = CreateHomeNavigationService();
            //homeNavigationService.Navigate();
            MainWindow = new MainWindow()
            {
                DataContext = new WindowViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService<HomeViewModel> CreateHomeNavigationService()
        {
            return new LayoutNavigationService<HomeViewModel>
                (_navigationStore,
                () => new HomeViewModel
                    (_accountStore, CreateLoginNavigationService(),
                    CreateChatNavigationService(),
                    CreateAccountNavigationService()),
                    GetNavBar);
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
                GetNavBar);
        }
        private INavigationService<ChatViewModel> CreateChatNavigationService()
        {
            return new LayoutNavigationService<ChatViewModel>
                (_navigationStore,
                () => new ChatViewModel
                    (_accountStore,
                    CreateAccountNavigationService()),
                GetNavBar);
        }
        private INavigationService<LoginViewModel> CreateLoginNavigationService()
        {
            return new NavigationService<LoginViewModel>
                (_navigationStore,
                () => new LoginViewModel
                    (_accountStore,
                    _navigationBarViewModel,
                    CreateAccountNavigationService(), CreateSignUpNavigationService()));
        }
        private INavigationService<SignUpViewModel> CreateSignUpNavigationService()
        {
            return new NavigationService<SignUpViewModel>
                (_navigationStore,
                () => new SignUpViewModel(_accountStore, CreateLoginNavigationService()));
        }
    }
}
