using ChatApp.Stores;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Commands
{
    class NavigateSignUpCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;
        private readonly NavigationBarViewModel _navigationBarViewModel;

        public NavigateSignUpCommand(NavigationStore navigationStore, AccountStore accountStore, NavigationBarViewModel navigationBarViewModel)
        {
            _navigationStore = navigationStore;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new SignUpViewModel(_navigationStore, _accountStore, _navigationBarViewModel);
        }
    }
}
