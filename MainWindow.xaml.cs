using System.Windows;
using System.Windows.Input;
using ChatApp.ViewModels;
using ChatApp.Views;

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



            //var store = new NavStore();

            //INavService toLogin = null!;
            //INavService toChat = null!;

            //toLogin = new NavigationService<LoginViewModel>(
            //    store,
            //    () => new LoginViewModel());

            //toChat = new NavigationService<ChatViewModel>(
            //    store,
            //    () => new ChatViewModel());

            //toLogin.Navigate();

            //DataContext = new WindowViewModel(store, toLogin);
            //main.Content = new LoginPage();
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

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);

        }
    }
}