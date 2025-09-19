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
                CreateLoginNavigationService());
        }


        protected override void OnStartup(StartupEventArgs e)
        {

            NavigationService<HomeViewModel> homeNavigationService = CreateHomeNavigationService();
            homeNavigationService.Navigate();
            MainWindow = new MainWindow()
            {
                DataContext = new WindowViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private NavigationService<HomeViewModel> CreateHomeNavigationService()
        {
            return new NavigationService<HomeViewModel>
                (_navigationStore,
                () => new HomeViewModel
                    (_navigationBarViewModel, CreateLoginNavigationService()));
        }
        private NavigationService<AccountViewModel> CreateAccountNavigationService()
        {
            return new NavigationService<AccountViewModel>
                (_navigationStore,
                () => new AccountViewModel
                    (
                    _accountStore,
                    CreateHomeNavigationService()));
        }

        private NavigationService<ChatViewModel> CreateChatNavigationService()
        {
            return new NavigationService<ChatViewModel>
                (_navigationStore,
                () => new ChatViewModel
                    (_navigationStore,
                    _accountStore,
                    _navigationBarViewModel));
        }
        private NavigationService<LoginViewModel> CreateLoginNavigationService()
        {
            return new NavigationService<LoginViewModel>
                (_navigationStore,
                () => new LoginViewModel
                    (_accountStore,
                    CreateAccountNavigationService()));
        }
    }

}
