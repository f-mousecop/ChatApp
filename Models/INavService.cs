using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public interface INavService
    {
        void Navigate();
    }

    public class NavigationService<TViewModel> : INavService
        where TViewModel : BaseViewModel
    {
        private readonly NavStore _store;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavStore store, Func<TViewModel> factory)
        {
            _store = store;
            _createViewModel = factory;
        }

        public void Navigate()
        {
            _store.CurrentViewModel = _createViewModel();
        }
    }
}
