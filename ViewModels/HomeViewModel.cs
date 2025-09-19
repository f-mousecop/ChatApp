using ChatApp.Commands;
using ChatApp.Models;
using ChatApp.Services;
using ChatApp.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly AccountStore _accountStore;
        private UserAccountModel _currentUserAccount;
        public NavigationBarViewModel NavigationBarViewModel { get; }

        public UserAccountModel CurrentUserAccount
        {
            get => _currentUserAccount;
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        // Commands
        public ICommand NavigateChatCommand { get; }
        public HomeViewModel(

            NavigationBarViewModel navigationBarViewModel,
            NavigationService<ChatViewModel> chatNavigationService)
        {
            NavigationBarViewModel = navigationBarViewModel;
            //_accountStore = accountStore;
            CurrentUserAccount = new UserAccountModel();

            var acct = _accountStore?.CurrentUserAccount;
            if (acct != null)
            {
                _accountStore.CurrentUserAccount.DisplayName = "No account found. Please log in";
            }
            CurrentUserAccount = acct;

            NavigateChatCommand = new NavigateCommand<ChatViewModel>(chatNavigationService);

        }

    }
}
