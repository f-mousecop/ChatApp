using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatApp.Models
{
    public class UserAccountModel : INotifyPropertyChanged
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
        private string? _avatarUrl = string.Empty;
        public string? AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                if (_avatarUrl != value)
                {
                    _avatarUrl = value;
                    OnPropertyChanged();    // AvatarUrl changed
                    OnPropertyChanged(nameof(AvatarImage));   // And the computed image changed too
                }
            }
        }


        // For image bindings
        public ImageSource? AvatarImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AvatarUrl)) return null;
                try
                {
                    var path = System.IO.Path.IsPathRooted(AvatarUrl!)
                        ? AvatarUrl!
                        : System.IO.Path.Combine(GetAppDataAvatarBase(), AvatarUrl!);

                    // Force a fresh load
                    using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = fs;
                    //bmp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    //bmp.UriSource = new System.Uri(path, System.UriKind.Absolute);
                    bmp.EndInit();
                    bmp.Freeze();
                    return bmp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Avatar Image Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
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

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
