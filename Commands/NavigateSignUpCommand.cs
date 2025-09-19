using ChatApp.Services;
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
        private readonly LoginViewModel _viewModel;
        private readonly AccountStore _accountStore;
        private readonly NavigationService<LoginViewModel> _navigationService;

        public NavigateSignUpCommand
            (LoginViewModel viewModel, AccountStore accountStore, NavigationService<LoginViewModel> navigationService)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;

        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
