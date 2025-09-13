using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.ViewModels
{
    public class WindowViewModel : BaseViewModel
    {
        //private ApplicationPage _currentPage = ApplicationPage.Login;

        public NavStore Nav { get; }

        #region Public Properties
        /// <summary>
        /// The current page of the application
        /// </summary>
        //public ApplicationPage CurrentPage
        //{
        //    get => _currentPage;
        //    set => SetProperty(ref _currentPage, value);
        //}
        #endregion

        public WindowViewModel(NavStore store, INavService toLogin)
        {
            Nav = store;
            toLogin.Navigate();
        }
    }
}
