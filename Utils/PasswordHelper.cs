using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp.Utils
{
    /// <summary>
    /// Password helper class for PasswordBox binding
    /// </summary>
    public static class PasswordHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject obj) =>
            (string)obj.GetValue(BoundPasswordProperty);

        // Change the type of the value parameter in SetBoundPassword from string to SecureString
        public static void SetBoundPassword(DependencyObject obj, string value) =>
            obj.SetValue(BoundPasswordProperty, value);

        private static void OnBoundPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not PasswordBox pb) return;

            pb.PasswordChanged -= HandlePasswordChanged;

            var newPassword = e.NewValue as string ?? string.Empty;
            if (newPassword != null)
            {
                if (pb.Password != newPassword)
                {
                    pb.Password = newPassword;
                }
            }

            pb.PasswordChanged += HandlePasswordChanged;
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                SetBoundPassword(pb, pb.Password);
            }
        }

        private static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                return null;
            var secure = new SecureString();
            foreach (char c in password)
                secure.AppendChar(c);
            secure.MakeReadOnly();
            return secure;
        }

        private static string ConvertToUnSecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(unmanagedString);
            }
            finally
            {
                if (unmanagedString != IntPtr.Zero)
                    System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(unmanagedString);
            }
        }
    }
}
