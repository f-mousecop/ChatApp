using ChatApp.Models;
using ChatApp.Stores;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace ChatApp.ViewModels
{
    public class WindowViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalStore;
        private readonly IUserRepository _userRepository;
        private Brush _shellBackground = Brushes.Transparent;
        public Brush ShellBackground
        {
            get => _shellBackground;
            set => SetProperty(ref _shellBackground, value);
        }
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public BaseViewModel CurrentModalViewModel => _modalStore.CurrentViewModel;
        public bool IsModalOpen => _modalStore.IsOpen;

        public WindowViewModel(NavigationStore navigationStore, ModalNavigationStore modalStore)
        {
            _navigationStore = navigationStore;
            _modalStore = modalStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;

            ApplyHostBackground(GetLeafViewModel(_navigationStore.CurrentViewModel));
        }


        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            ApplyHostBackground(GetLeafViewModel(_navigationStore.CurrentViewModel));
        }

        private static BaseViewModel GetLeafViewModel(BaseViewModel viewModel)
            => (viewModel as LayoutViewModel)?.ContentViewModel ?? viewModel;


        private void ApplyHostBackground(BaseViewModel currentViewModel)
        {
            Debug.WriteLine($"VM type: + {currentViewModel?.GetType().FullName ?? "<null>"}");

            switch (currentViewModel)
            {
                case HomeViewModel:
                    ShellBackground = (Brush)Application.Current.FindResource("HomeShellBrush");
                    return;

                case AccountViewModel:
                    ShellBackground = (Brush)Application.Current.FindResource("BackgroundDarkBrush");
                    return;

                case ChatViewModel:
                    ShellBackground = (Brush)Application.Current.FindResource("CadetGrayBrush");
                    return;

                case SignUpViewModel:
                    ShellBackground = (Brush)Application.Current.FindResource("WindowsBackgroundBrush");
                    return;

                default:
                    ShellBackground = (Brush)Application.Current.FindResource("WindowChromeBackgroundBrush");
                    return;
            }
        }


        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }
    }
}
