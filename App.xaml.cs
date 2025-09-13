using ChatApp.Stores;
using ChatApp.ViewModels;
using System.Windows;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

            navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);
            MainWindow = new MainWindow()
            {
                DataContext = new WindowViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
