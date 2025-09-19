

using ChatApp.Commands;
using ChatApp.Services;
using ChatApp.Stores;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public string ConfirmPass { get; set; }
        private readonly NavigationStore _navigationStore;

        public ICommand NavigateSignUpCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public SignUpViewModel(
            AccountStore accountStore,
            NavigationBarViewModel navigationBarViewModel,
            NavigationService<LoginViewModel> loginNavigationService)
        {
            NavigateBackCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
        }
    }
}
