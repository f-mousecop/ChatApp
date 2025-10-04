using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatApp.Models
{
    public class UserAccountModel
    {
        // Database identity
        public int Id { get; set; }

        // Core identity
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Names
        public string DisplayName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        // Path/url for avatar image
        public string? AvatarUrl { get; set; } = string.Empty;


        // For image bindings
        public ImageSource? AvatarImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AvatarUrl)) return null;
                try
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.UriSource = System.IO.Path.IsPathRooted(AvatarUrl)
                        ? new System.Uri(AvatarUrl, System.UriKind.Absolute)
                        : new System.Uri(System.IO.Path.Combine(GetAppDataAvatarBase(), AvatarUrl), System.UriKind.Absolute);
                    bmp.EndInit();
                    bmp.Freeze();
                    return bmp;
                }
                catch { return null; }
            }
        }

        // Saved in relative path for now
        private static string GetAppDataAvatarBase()
        {
            var baseDir = System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                "ChatApp");
            return baseDir;
        }
    }
}
