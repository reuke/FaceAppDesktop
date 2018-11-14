using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FaceAppApi.V3
{
    public static class Helper
    {
        private static readonly Random Random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        
        public static string RandomNumberString(int length)
        {
            const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static BitmapSource ToBitmapImage(this byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public static BitmapSource CropBitmapImage(BitmapSource baseImage)
        {
            int imageWidth = baseImage.PixelWidth / 2;
            int imageHeight = baseImage.PixelHeight;

            return new CroppedBitmap(baseImage,
                new Int32Rect(
                    imageWidth, 0,
                    imageWidth, imageHeight
                ));
        }
    }
}