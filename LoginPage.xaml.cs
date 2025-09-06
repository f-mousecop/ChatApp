using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void UserLogin_Click(object sender, RoutedEventArgs e)
        {
            var userName = userNameBox.Text;
            if (!string.IsNullOrEmpty(userName))
            {
                NavigationService.Navigate(new Chat());
            }
            else
            {
                MessageBox.Show("Please Enter Username and Password", "Login Fail", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
