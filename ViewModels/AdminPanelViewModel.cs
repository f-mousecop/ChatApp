using ChatApp.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class AdminPanelViewModel : BaseViewModel
    {
        private readonly AccountStore _accountStore;
        public AdminPanelViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;
        }
    }
}
