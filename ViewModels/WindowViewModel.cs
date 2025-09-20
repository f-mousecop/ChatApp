using ChatApp.Models;
using ChatApp.Stores;

namespace ChatApp.ViewModels
{
    public class WindowViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalStore;
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public BaseViewModel CurrentModalViewModel => _modalStore.CurrentViewModel;
        public bool IsModalOpen => _modalStore.IsOpen;

        public WindowViewModel(NavigationStore navigationStore, ModalNavigationStore modalStore)
        {
            _navigationStore = navigationStore;
            _modalStore = modalStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }
    }
}
