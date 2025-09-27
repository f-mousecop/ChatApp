using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ChatApp.Utils
{
    // DWM interop enums (subset)
    public enum DWMWINDOWATTRIBUTE
    {
        NCRenderingPolicy = 2,
        UseImmersiveDarkMode_Old = 19,  // some Win10 builds
        UseImmersiveDarkMode = 20,  // Win10+ newer / Win11
        WindowCornerPreference = 33,  // Win11+
    }

    public enum DWMNCRENDERINGPOLICY
    {
        UseWindowStyle = 0,
        Disabled = 1,
        Enabled = 2,
    }

    public enum DWM_WINDOW_CORNER_PREFERENCE
    {
        Default = 0,
        DoNotRound = 1,
        Round = 2,
        RoundSmall = 3,
    }

    public static class TestDwmShadow
    {
        // Attached properties
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached(
                "Enable",
                typeof(bool),
                typeof(DwmShadow),
                new PropertyMetadata(false, OnEnableChanged));

        public static readonly DependencyProperty DarkModeProperty =
            DependencyProperty.RegisterAttached(
                "DarkMode",
                typeof(bool),
                typeof(DwmShadow),
                new PropertyMetadata(true, OnDarkModeChanged)); // default on (harmless)

        public static readonly DependencyProperty CornerPreferenceProperty =
            DependencyProperty.RegisterAttached(
                "CornerPreference",
                typeof(DWM_WINDOW_CORNER_PREFERENCE),
                typeof(DwmShadow),
                new PropertyMetadata(DWM_WINDOW_CORNER_PREFERENCE.Round, OnCornerChanged));

        public static void SetEnable(DependencyObject obj, bool value) => obj.SetValue(EnableProperty, value);
        public static bool GetEnable(DependencyObject obj) => (bool)obj.GetValue(EnableProperty);

        public static void SetDarkMode(DependencyObject obj, bool value) => obj.SetValue(DarkModeProperty, value);
        public static bool GetDarkMode(DependencyObject obj) => (bool)obj.GetValue(DarkModeProperty);

        public static void SetCornerPreference(DependencyObject obj, DWM_WINDOW_CORNER_PREFERENCE value) => obj.SetValue(CornerPreferenceProperty, value);
        public static DWM_WINDOW_CORNER_PREFERENCE GetCornerPreference(DependencyObject obj) => (DWM_WINDOW_CORNER_PREFERENCE)obj.GetValue(CornerPreferenceProperty);

        // Handlers
        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window win) return;

            if ((bool)e.NewValue == false)
            {
                // Optionally disable NC rendering if you really want to remove shadows
                // (OS may ignore this hint)
                SafeWhenSourced(win, DisableNcRendering);
                return;
            }

            SafeWhenSourced(win, ApplyAll);
        }

        private static void OnDarkModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window win) return;
            if (!GetEnable(win)) return; // only act when enabled

            SafeWhenSourced(win, w => ApplyDarkMode(w, (bool)e.NewValue));
        }

        private static void OnCornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window win) return;
            if (!GetEnable(win)) return;

            SafeWhenSourced(win, w => ApplyCorners(w, (DWM_WINDOW_CORNER_PREFERENCE)e.NewValue));
        }

        // Core apply
        private static void ApplyAll(Window w)
        {
            ApplyNcRendering(w, DWMNCRENDERINGPOLICY.Enabled);
            ApplyDarkMode(w, GetDarkMode(w));
            ApplyCorners(w, GetCornerPreference(w));
        }

        private static void DisableNcRendering(Window w)
        {
            ApplyNcRendering(w, DWMNCRENDERINGPOLICY.Disabled);
        }

        private static void ApplyNcRendering(Window w, DWMNCRENDERINGPOLICY policy)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            if (hwnd == IntPtr.Zero) return;

            int val = (int)policy;
            _ = DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.NCRenderingPolicy, ref val, sizeof(int));
        }

        private static void ApplyDarkMode(Window w, bool on)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            if (hwnd == IntPtr.Zero) return;

            int v = on ? 1 : 0;
            // Try newer id first, then older
            _ = DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.UseImmersiveDarkMode, ref v, sizeof(int));
            _ = DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.UseImmersiveDarkMode_Old, ref v, sizeof(int));
        }

        private static void ApplyCorners(Window w, DWM_WINDOW_CORNER_PREFERENCE pref)
        {
            if (!IsWin11OrGreater()) return; // Corner preference is Win11+
            var hwnd = new WindowInteropHelper(w).Handle;
            if (hwnd == IntPtr.Zero) return;

            int v = (int)pref;
            _ = DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.WindowCornerPreference, ref v, sizeof(int));
        }

        // Utility: run now or after SourceInitialized (once)
        private static void SafeWhenSourced(Window w, Action<Window> action)
        {
            if (w.IsInitialized && PresentationSource.FromVisual(w) is HwndSource)
            {
                action(w);
            }
            else
            {
                void Handler(object? s, EventArgs e)
                {
                    w.SourceInitialized -= Handler;
                    action(w);
                }
                w.SourceInitialized += Handler;
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, int cbAttribute);

        // Basic OS check; corner pref is honored on 10 only in Insider/none; officially 11+
        private static bool IsWin11OrGreater()
        {
            // Windows 11 is version 10.0 build >= 22000
            Version v = Environment.OSVersion.Version;
            return (v.Major > 10) || (v.Major == 10 && v.Build >= 22000);
        }
    }
}

