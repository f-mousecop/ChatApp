

using ChatApp.Commands;
using ChatApp.Stores;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public string ConfirmPass { get; set; }

        public ICommand NavigateSignUpCommand { get; }

        public SignUpViewModel(NavigationStore navigationStore)
        {
            NavigateSignUpCommand = new NavigateSignUpCommand(navigationStore);
        }

    }
}
