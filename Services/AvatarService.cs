using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatApp.Services
{
    public class AvatarService
    {
        // Avatar images are stored on the disk
        public static string AvatarsBaseDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChatApp", "avatars");

        /// <summary>
        /// Copies and resizes the selected image into the app avatar folder
        /// using <see cref="userId.jpg"/> returns the relative path to store in the DB: "avatars/{userId}.jpg"
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sourcePath"></param>
        /// <param name="maxSizePx"></param>
        /// <returns></returns>
        public static async Task<string> SaveAvatarAsync(int userId, string sourcePath, int maxSizePx = 512)
        {
            Directory.CreateDirectory(AvatarsBaseDir);

            var destFile = Path.Combine(AvatarsBaseDir, $"{userId}.jpg");

            // Load, resize if needed, and save as jpeg
            using var src = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.StreamSource = src;
            bmp.EndInit();
            bmp.Freeze();

            // Compute scale
            var scale = Math.Max(bmp.PixelHeight, bmp.PixelWidth) > maxSizePx
                ? (double)maxSizePx / Math.Max(bmp.PixelHeight, bmp.PixelWidth)
                : 1.0;

            BitmapSource toSave = bmp;
            if (scale < 1.0)
            {
                var tb = new TransformedBitmap(bmp, new ScaleTransform(scale, scale));
                tb.Freeze();
                toSave = tb;
            }

            // Encode as jpeg
            var encoder = new JpegBitmapEncoder { QualityLevel = 85 };
            encoder.Frames.Add(BitmapFrame.Create(toSave));
            using (var fs = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
                encoder.Save(fs);

            // Return relative path 
            return $"avatars/{userId}.jpg";
        }
    }
}
