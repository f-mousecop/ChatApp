using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatApp.Utils
{
    public static class OnLoadedCommand
    {
        public static readonly DependencyProperty OnLoadedCommandProperty =
            DependencyProperty.RegisterAttached(
                "LoadedCommand", typeof(ICommand), typeof(OnLoadedCommand),
                new PropertyMetadata(null, OnChanged));

        public static void SetLoadedCommand(DependencyObject d, ICommand command) => d.SetValue(OnLoadedCommandProperty, command);

        public static ICommand GetLoadedCommand(DependencyObject d) => (ICommand)d.GetValue(OnLoadedCommandProperty);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement frameworkElement)
            {
                frameworkElement.Loaded -= FrameworkElementOnLoaded;
                if (e.NewValue is ICommand) frameworkElement.Loaded += FrameworkElementOnLoaded;
            }
        }

        private static void FrameworkElementOnLoaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;
            var cmd = GetLoadedCommand(frameworkElement);
            if (cmd?.CanExecute(null) == true) cmd.Execute(null);
        }
    }
}
