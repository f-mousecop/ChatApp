using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services;
using ChatApp.Stores;
using ChatApp.Utils;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        private readonly AccountStore _accountStore;
        private readonly ThemeService _themeService;

        public bool IsDarkTheme
        {
            get => _themeService.IsDarkTheme;
            set => _themeService.IsDarkTheme = value;
        }

        private bool _isToggled;
        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (SetProperty(ref _isToggled, value))
                {
                    OnPropertyChanged(nameof(IsToggled));
                }
            }
        }

        public ICommand NavigateChatCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateLogoutCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateSignUpCommand { get; }
        public ICommand NavigateAdminPanelCommand { get; }
        public ICommand ChangeThemeCommand { get; }

        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public bool IsNotLoggedIn => _accountStore.IsNotLoggedIn;
        public string? CurrentUserAccount => _accountStore.Username;
        public bool IsAdmin => _accountStore.IsAdmin;

        public NavigationBarViewModel(
            AccountStore accountStore,
            INavigationService navigateAdminPanelService,
            INavigationService homeNavigationService,
            INavigationService chatNavigationService,
            INavigationService accountNavigationService,
            INavigationService loginNavigationService,
            INavigationService signUpNavigationService,
            ThemeService themeService)
        {
            _accountStore = accountStore;
            _themeService = themeService;

            NavigateAdminPanelCommand = new NavigateCommand(navigateAdminPanelService);
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateChatCommand = new NavigateCommand(chatNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);

            NavigateLogoutCommand = new RelayCommand(
                _ => { _accountStore.Logout(); homeNavigationService.Navigate(); },
                _ => _accountStore.IsLoggedIn);
            NavigateSignUpCommand = new NavigateCommand(signUpNavigationService);
            NavigateLoginCommand = new NavigateCommand(loginNavigationService);

            //ChangeThemeCommand = new RelayCommand(_ => ExecuteChangeThemeCommand());

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;

        }

        //private void ExecuteChangeThemeCommand()
        //{
        //    //var palette = new PaletteHelper();

        //    //Theme theme = palette.GetTheme();

        //    //if (IsToggled)
        //    //{
        //    //    theme.SetBaseTheme(BaseTheme.Dark);
        //    //}
        //    //else
        //    //{
        //    //    theme.SetBaseTheme(BaseTheme.Light);
        //    //}
        //    //palette.SetTheme(theme);

        //    var themeService = new ThemeService();
        //    themeService.IsDarkTheme = true;
        //}

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
