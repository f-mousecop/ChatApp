using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ChatApp.Utils
{
    public static class DwmShadow
    {
        // Attached property <Window utils:DwmShadow.Enable="True" />
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached(
                "Enable",
                typeof(bool),
                typeof(DwmShadow),
                new PropertyMetadata(false, OnEnableChanged));

        public static void SetEnable(DependencyObject d, bool value) => d.SetValue(EnableProperty, value);
        public static bool GetEnable(DependencyObject d) => (bool)d.GetValue(EnableProperty);

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) return;
            if ((bool)e.NewValue == false) return;

            // IF window is already sourced, apply now, else wait
            if (window.IsInitialized && PresentationSource.FromVisual(window) is HwndSource)
            {
                Apply(window);
            }
            else
            {
                // Run once, then detach
                window.SourceInitialized += Win_SourceInitialized;
            }
        }

        private static void Win_SourceInitialized(object? sender, EventArgs e)
        {
            if (sender is Window window)
            {
                window.SourceInitialized -= Win_SourceInitialized;
                Apply(window);
            }
        }

        public static void Apply(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero) return;

            // DWM draws the non-client area
            const int DWMWA_NCRENDERING_POLICY = 2;
            int DWMNCRP_ENABLED = 2;
            _ = DwmSetWindowAttribute(hwnd, DWMWA_NCRENDERING_POLICY, ref DWMNCRP_ENABLED, sizeof(int));

            // Dark mode
            int useDark = 1;
            _ = DwmSetWindowAttribute(hwnd, 20, ref useDark, sizeof(int));
            _ = DwmSetWindowAttribute(hwnd, 19, ref useDark, sizeof(int));

            // Rounded corners
            const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
            int DWMWCP_ROUND = 2;
            _ = DwmSetWindowAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref DWMWCP_ROUND, sizeof(int));
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    }
}
