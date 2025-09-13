using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ChatApp.ViewModels;

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

        // Initialize NavCommand in the constructor to fix CS8618
        public ChatViewModel()
        {
            GetUserNameCommand = new RelayCommand(_ => { /* TODO */ });
            GetUserMessageCommand = new RelayCommand(_ => { /* TODO */ });
            GetBotMessageCommand = new RelayCommand(_ => { /* TODO */ });
            GetErrorMessageCommand = new RelayCommand(_ => { /* TODO */ });
            NavCommand = new RelayCommand(_ => CanExecuteNav());
        }

        public async Task CanExecuteNav()
        {
            try
            {

                await Task.Delay(1);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
