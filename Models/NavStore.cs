using ChatApp.ViewModels;

namespace ChatApp.Models
{
    public class NavStore : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                SetProperty(ref _currentViewModel, value);
            }
        }
    }
}
