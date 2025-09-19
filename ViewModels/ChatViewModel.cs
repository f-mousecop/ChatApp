using ChatApp.Commands;
using ChatApp.Services;
using ChatApp.Stores;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        // Initialize non-nullable fields to default values to fix CS8618
        private string _username = string.Empty;
        private string _userMessage = string.Empty;
        private string _botMessage = string.Empty;
        private string _errorMessage = string.Empty;

        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                if (SetProperty(ref _username, value))
                    ((RelayCommand)GetUserNameCommand).RaiseCanExecuteChanged();
            }
        }

        public string UserMessage
        {
            get { return _userMessage; }
            set
            {
                if (SetProperty(ref _userMessage, value))
                    ((RelayCommand)GetUserMessageCommand).RaiseCanExecuteChanged();
            }
        }

        public string BotMessage
        {
            get { return _botMessage; }
            set
            {
                if (SetProperty(ref _botMessage, value))
                    ((RelayCommand)GetBotMessageCommand).RaiseCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (SetProperty(ref _errorMessage, value))
                    ((RelayCommand)GetErrorMessageCommand).RaiseCanExecuteChanged();
            }
        }


        public ICommand GetUserNameCommand { get; }
        public ICommand GetUserMessageCommand { get; }
        public ICommand GetBotMessageCommand { get; }
        public ICommand GetErrorMessageCommand { get; }
        public ICommand NavCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public NavigationBarViewModel NavigationBarViewModel { get; }

        // Initialize NavCommand in the constructor to fix CS8618
        public ChatViewModel(AccountStore accountStore,
            NavigationBarViewModel navigationBarViewModel,
            NavigationService<HomeViewModel> homeNavigationService,
            NavigationService<AccountViewModel> accountNavigationService)
        {
            NavigationBarViewModel = navigationBarViewModel;

            GetUserNameCommand = new RelayCommand(_ => { /* TODO */ });
            GetUserMessageCommand = new RelayCommand(_ => { /* TODO */ });
            GetBotMessageCommand = new RelayCommand(_ => { /* TODO */ });
            GetErrorMessageCommand = new RelayCommand(_ => { /* TODO */ });

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(homeNavigationService);
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(accountNavigationService);
        }
    }
}
