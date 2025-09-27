using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ChatApp.Utils
{
    public static class HwndDebug
    {
        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x00080000;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        public static bool IsLayered(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            var ex = GetWindowLongPtr(hwnd, GWL_EXSTYLE).ToInt64();
            return (ex & WS_EX_LAYERED) != 0;
        }
    }
}
