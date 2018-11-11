using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FaceApp
{
    internal static class Helpers
    {
        private const int MaxPath = 260;
        public static string TempFolderPath = Path.Combine(Path.GetTempPath(), "FaceAppTempV2");

        public static string AdaptPath(string path)
        {
            if (path.Length < MaxPath)
                return path;

            var delimiterIndex = path.LastIndexOf("\\");

            if (delimiterIndex == -1)
                return null;

            var dir = path.Substring(0, delimiterIndex);
            var file = path.Substring(delimiterIndex + 1);

            var fileLen = MaxPath - dir.Length - 2;

            if (fileLen < 5)
                return null;

            file = file.Substring(file.Length - fileLen);

            return Path.Combine(dir, file);
        }

        public static void SaveFile(Bitmap image, string path)
        {
            if (path == null)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var filestream = new FileStream(path, FileMode.Create))
            {
                image.Save(filestream, ImageFormat.Jpeg);
            }
        }

        public static void SaveFile(BitmapImage image, string path)
        {
            if (path == null)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var filestream = new FileStream(path, FileMode.Create))
            {
                encoder.Save(filestream);
            }
        }

        public static void DragCopyFile(DependencyObject source, string path)
        {
            if (path != null)
            {
                var dragObject = new DataObject();
                dragObject.SetFileDropList(new StringCollection {path});
                DragDrop.DoDragDrop(source, dragObject, DragDropEffects.Copy);
            }
        }
    }
}