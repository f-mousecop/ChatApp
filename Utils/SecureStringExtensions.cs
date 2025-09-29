using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Utils
{
    public static class SecureStringExtensions
    {
        public static string ToUnsecureString(this SecureString value)
        {
            if (value == null) return string.Empty;
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToBSTR(value);
                return Marshal.PtrToStringBSTR(ptr) ?? string.Empty;
            }
            finally
            {
                if (ptr != IntPtr.Zero) Marshal.ZeroFreeBSTR(ptr);
            }
        }
    }
}
