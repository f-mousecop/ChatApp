using ChatApp.Stores;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class LayoutNavigationService<TViewModel> : INavigationService<TViewModel>
        where TViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<NavigationBarViewModel> _getNavigationBarViewModel;
        private readonly Func<TViewModel> _createViewModel;

        public LayoutNavigationService(
            NavigationStore navigationStore,
            Func<TViewModel> createViewModel,
            Func<NavigationBarViewModel> getNavigationBarViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _getNavigationBarViewModel = getNavigationBarViewModel;
        }

        public void Navigate()
        {
            var navBar = _getNavigationBarViewModel(); // Resolved now (not during App ctor
            _navigationStore.CurrentViewModel = new LayoutViewModel(navBar, _createViewModel());
        }
    }
}
