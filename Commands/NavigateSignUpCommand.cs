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

        public NavigateSignUpCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new SignUpViewModel(_navigationStore);
        }
    }
}
