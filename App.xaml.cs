using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using ChatApp.Utils;
using ChatApp.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; }
        private readonly AccountStore _accountStore = new();
        private IUserRepository? _userRepository;
        private readonly NavigationStore _navigationStore = new();
        private readonly ModalNavigationStore _modalNavigationStore = new();

        private ThemeService _themeService;


        protected override void OnStartup(StartupEventArgs e)
        {

            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            _userRepository = new UserRepository();

            _themeService = new ThemeService();

            // Nav to home
            var homeSvc = CreateHomeNavigationService();
            homeSvc.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new WindowViewModel(_navigationStore, _modalNavigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }


        private INavigationService CreateHomeNavigationService()
        {
            return new LayoutNavigationService<HomeViewModel>
                (_navigationStore,
                () => new HomeViewModel
                    (_accountStore, CreateLoginNavigationService(),
                    CreateChatNavigationService(),
                    CreateAccountNavigationService()),
                    CreateNavigationBarViewModel);
        }

        private INavigationService CreateAdminPanelNavigationService()
        {
            return new LayoutNavigationService<AdminPanelViewModel>
                (_navigationStore,
                () => new AdminPanelViewModel(
                    _accountStore, _userRepository), CreateNavigationBarViewModel);
        }


        private INavigationService CreateAccountNavigationService()
        {
            return new LayoutNavigationService<AccountViewModel>
                (_navigationStore,
                () => new AccountViewModel
                    (
                    _accountStore,
                    CreateHomeNavigationService(),
                    CreateChatNavigationService(),
                    CreateAvatarViewNavigationService()),
                CreateNavigationBarViewModel);
        }
        private INavigationService CreateChatNavigationService()
        {
            return new LayoutNavigationService<ChatViewModel>
                (_navigationStore,
                () => new ChatViewModel
                    (_accountStore,
                    CreateAccountNavigationService()),
                CreateNavigationBarViewModel);
        }

        private INavigationService CreateLoginNavigationService()
        {
            //CompositeNavigationService navigationService = new CompositeNavigationService(
            //    new CloseModalNavigationService(_modalNavigationStore),
            //    CreateAccountNavigationService()
            //    );

            var closeModalSvc = new CloseModalNavigationService(_modalNavigationStore);

            return new ModalNavigationService<LoginViewModel>
                (_modalNavigationStore,
                () => new LoginViewModel(
                    _accountStore,
                    closeModalSvc,
                    CreateAccountNavigationService(),
                    CreateAdminPanelNavigationService(),
                    CreateSignUpNavigationService()));

        }

        private INavigationService CreateSignUpNavigationService()
        {
            return new ModalNavigationService<SignUpViewModel>
                (_modalNavigationStore,
                () => new SignUpViewModel(_accountStore, CreateLoginNavigationService()));
        }

        private INavigationService CreateAvatarViewNavigationService()
        {
            var closeModalSvc = new CloseModalNavigationService(_modalNavigationStore);

            return new ModalNavigationService<AvatarViewModel>
                (_modalNavigationStore,
                () => new AvatarViewModel(_accountStore, closeModalSvc, CreateAccountNavigationService()));
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            return new NavigationBarViewModel(_accountStore,
                CreateAdminPanelNavigationService(),
                CreateHomeNavigationService(),
                CreateChatNavigationService(),
                CreateAccountNavigationService(),
                CreateLoginNavigationService(),
                CreateSignUpNavigationService(),
                _themeService);
        }
    }
}
