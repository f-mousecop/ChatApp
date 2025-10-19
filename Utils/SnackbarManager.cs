using ChatApp.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ChatApp.Utils
{
    public class SnackbarManager : BaseViewModel
    {
        private static readonly Lazy<SnackbarManager> _lazy =
            new(() => new SnackbarManager());
        public static SnackbarManager Instance => _lazy.Value;

        private readonly Dispatcher _ui;
        public class MessageToSnack
        {
            public string Content { get; set; } = string.Empty;
            public MessageToSnackLevel Level { get; set; } = 0;
            public TimeSpan? Duration { get; set; } = null;
            public bool WithCloseButton { get; set; } = true;
        }

        public enum MessageToSnackLevel
        {
            None = 0,
            Error = 1,
            Warning = 2,
            Info = 3,
            Success = 4,
        }


        public SnackbarManager() : this(new SnackbarMessageQueue(TimeSpan.FromSeconds(5))) { }

        public SnackbarManager(SnackbarMessageQueue messageQueue)
        {
            _ui = Dispatcher.CurrentDispatcher;
            _messageQueue = messageQueue;
        }

        private SnackbarMessageQueue _messageQueue;
        public SnackbarMessageQueue MessageQueue
        {
            get => _messageQueue;
            set => SetProperty(ref _messageQueue, value);
        }

        public void Enqueue(string content, MessageToSnackLevel level = MessageToSnackLevel.Info,
            TimeSpan? duration = null, bool withCloseButton = true)
        {
            void DoEnqueue()
            {
                var message = new MessageToSnack
                {
                    Content = content,
                    Level = level,
                    Duration = duration,
                    WithCloseButton = withCloseButton
                };
                EventSnackMessageReceived(message);
            }

            if (_ui.CheckAccess()) DoEnqueue();
            else _ui.Invoke(DoEnqueue);

        }

        private void EventSnackMessageReceived(MessageToSnack messageToSnack)
        {
            _snackMessages.Add(messageToSnack);

            if (messageToSnack.WithCloseButton)
            {
                MessageQueue.Enqueue(messageToSnack.Content, "Close", () => HandleCloseAction());
            }
            else
            {
                MessageQueue.Enqueue(messageToSnack.Content, null, null, null, false, false, messageToSnack.Duration);
            }
        }

        private void HandleCloseAction()
        {
            MessageQueue.Clear();
        }

        List<MessageToSnack> _snackMessages = new();

        private SnackbarMessage _message;
        public SnackbarMessage Message
        {
            get => _message;
            set
            {
                SetProperty(ref _message, value);

                if (_message != null)
                {
                    var localMessage = _snackMessages.FirstOrDefault(m => m.Content.Equals(_message.Content.ToString()));
                    if (localMessage != null)
                    {
                        CurrentMessageLevel = localMessage.Level;
                        _snackMessages.Remove(localMessage);
                    }
                    else
                    {
                        CurrentMessageLevel = 0;
                    }
                }
                else
                {
                    CurrentMessageLevel = 0;
                }
            }
        }

        private MessageToSnackLevel _currentMessageLevel;
        public MessageToSnackLevel CurrentMessageLevel
        {
            get => _currentMessageLevel;
            set => SetProperty(ref _currentMessageLevel, value);
        }
    }
}
