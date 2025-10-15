using ChatApp.Commands;
using ChatApp.Services;
using ChatApp.Stores;
using ChatApp.Utils;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChatApp.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private readonly AccountStore _accountStore;
        private readonly OpenAiService _openAiService = new();
        private readonly MarkdownRenderer _markdownRenderer = new();
        private CancellationTokenSource? _runCts;

        // Initialize non-nullable fields to default values to fix CS8618
        private string _userMessage = string.Empty;
        private string _botMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _busy;
        private FlowDocument? _botMessageDocument;
        private DispatcherTimer _renderDebounce;

        public string? CurrentUserAccount => _accountStore.Username;

        public string UserMessage
        {
            get => _userMessage;
            set
            {
                if (SetProperty(ref _userMessage, value))
                {
                    ((RelayCommand)SendMessageCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)SendStreamCommand).RaiseCanExecuteChanged(); // * For streaming later *
                }
            }
        }

        public string BotMessage
        {
            get => _botMessage;
            set
            {
                if (SetProperty(ref _botMessage, value))
                    DebounceRenderer();
            }
        }



        public FlowDocument? BotMessageDocument
        {
            get => _botMessageDocument;
            private set => SetProperty(ref _botMessageDocument, value);
        }

        public bool IsBusy
        {
            get => _busy;
            set
            {
                if (SetProperty(ref _busy, value))
                {
                    ((RelayCommand)SendMessageCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)SendStreamCommand).RaiseCanExecuteChanged(); // * For streaming later *
                    ((RelayCommand)CancelCommand).RaiseCanExecuteChanged();     // * For streaming later *
                }
            }
        }



        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ObservableCollection<string> Messages { get; set; }



        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand SendStreamCommand { get; }
        public ICommand CancelCommand { get; }

        public NavigationBarViewModel NavigationBarViewModel { get; }

        // Initialize NavCommand in the constructor to fix CS8618
        public ChatViewModel(AccountStore accountStore,
            INavigationService accountNavigationService,
            INavigationService homeNavigationService)
        {
            //NavigationBarViewModel = navigationBarViewModel;
            Messages = new ObservableCollection<string>();

            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);

            /* Chat message commands */
            // Send message (non-streaming)
            SendMessageCommand = new RelayCommand(async _ => await SendMessageAsync(),
                                _ => !IsBusy && !string.IsNullOrWhiteSpace(UserMessage));

            // Send message (streaming)
            SendStreamCommand = new RelayCommand(async _ => await SendStreamingAsync(),
                                _ => !IsBusy && !string.IsNullOrWhiteSpace(UserMessage));

            // Cancel message command
            CancelCommand = new RelayCommand(_ => _runCts?.Cancel(), _ => IsBusy);
        }



        private async Task SendMessageAsync()
        {
            var text = UserMessage;
            if (string.IsNullOrWhiteSpace(text)) return;

            try
            {
                IsBusy = true;
                AppendLine($"> You: {text}");
                UserMessage = string.Empty;

                var reply = await _openAiService.GetReplyAsync(text);
                AppendAssistant(reply);
            }
            catch (OperationCanceledException)
            {
                AppendLine("[Canceled]");
            }
            catch (Exception ex)
            {
                AppendLine($"[Error] {ex.Message}");
            }
            finally { IsBusy = false; }
        }

        private async Task SendStreamingAsync()
        {
            var text = UserMessage;
            if (string.IsNullOrWhiteSpace(text)) return;

            _runCts?.Cancel();
            _runCts?.Dispose();
            _runCts = new CancellationTokenSource();


            try
            {
                IsBusy = true;
                AppendLine($"> You: {text}");
                UserMessage = string.Empty;

                // Start assistant section header once
                if (string.IsNullOrWhiteSpace(BotMessage))
                    BotMessage = "Assistant: ";
                else
                    BotMessage += "Assistant: ";

                await _openAiService.StreamReplyAsync(
                    text,
                    onToken: delta =>
                    {
                        // Streamed text delta arrives here (Responses API)
                        BotMessage += delta;
                    },
                    ct: _runCts.Token
                );

                // Finish the assistant block with spacing
                BotMessage += Environment.NewLine + Environment.NewLine;
            }
            catch (OperationCanceledException)
            {
                BotMessage += " [canceled] " + Environment.NewLine + Environment.NewLine;
            }
            catch (Exception ex)
            {
                AppendLine($"[Error] {ex.Message}");
            }
            finally { IsBusy = false; }
        }

        // Throttle markdown rendering so that it doesn't render on each tick
        private void DebounceRenderer()
        {
            if (_renderDebounce == null)
            {
                _renderDebounce ??= new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
                _renderDebounce.Tick += OnRenderTick;
            }

            _renderDebounce.Stop();
            _renderDebounce.Start();
        }

        private void OnRenderTick(object? sender, EventArgs e)
        {
            _renderDebounce.Stop();
            BotMessageDocument = _markdownRenderer.Render(_botMessage);
        }

        private void AppendAssistant(string reply)
        {
            if (string.IsNullOrWhiteSpace(reply))
                AppendLine("Assistant: (no response)");
            else
                AppendLine($"Assistant: {reply}");
        }

        private void AppendLine(string v)
        {
            if (string.IsNullOrWhiteSpace(BotMessage))
                BotMessage = v + Environment.NewLine + Environment.NewLine;
            else
                BotMessage += v + Environment.NewLine + Environment.NewLine;
        }

        public override void Dispose()
        {
            _runCts?.Cancel();
            _runCts?.Dispose();
        }
    }
}
