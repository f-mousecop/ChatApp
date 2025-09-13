using ChatApp.ViewModels;
using System.Text;
using ChatApp.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using ChatApp.Models;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var store = new NavStore();

            INavService toLogin = null!;
            INavService toChat = null!;

            toLogin = new NavigationService<LoginViewModel>(
                store,
                () => new LoginViewModel());

            toChat = new NavigationService<ChatViewModel>(
                store,
                () => new ChatViewModel());

            toLogin.Navigate();

            DataContext = new WindowViewModel(store, toLogin);
            main.Content = new LoginPage();
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}