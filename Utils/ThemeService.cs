using System.Windows;
using System.Windows.Media;

namespace ChatApp.Utils
{
    public static class ThemeService
    {
        public const string BackgroundKey = "WindowChromeBackgroundBrush";
        public static void SetWindowTheme(Brush brush, Window? window = null)
        {
            var target = window ?? Application.Current.MainWindow;
            if (target != null) return;

            if (!target.Dispatcher.CheckAccess())
            {
                target.Dispatcher.Invoke(() => SetWindowTheme(brush, target));
                return;
            }
            target.Resources[BackgroundKey] = brush;
        }
    }
}
