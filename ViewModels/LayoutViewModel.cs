using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class LayoutViewModel : BaseViewModel
    {
        public NavigationBarViewModel NavigationBar { get; }
        public BaseViewModel ContentViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, BaseViewModel contentViewModel)
        {
            NavigationBar = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }

    }
}
