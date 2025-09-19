// DEBUG
#if DEBUG
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace ChatApp.Components
{
    /// <summary>
    /// Interaction logic for Layout.xaml
    /// </summary>
    public partial class Layout : UserControl
    {
        public Layout()
        {
            InitializeComponent();
            this.Loaded += Layout_Loaded;
        }

        private void Layout_Loaded(object sender, RoutedEventArgs e)
        {
            int navCount = CountElementsofType<Components.NavigationBar>(this);
            Debug.Assert(navCount == 1, $"Found {navCount} Navigationbars. There should be exactly 1 (in layout).");
        }

        private static int CountElementsofType<T>(DependencyObject root)
        {
            int count = 0;
            int children = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < children; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child is T) count++;
                count += CountElementsofType<T>(child);
            }
            return count;
        }

    }
}
#endif
