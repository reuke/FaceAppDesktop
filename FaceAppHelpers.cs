using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FaceApp
{
    static class FaceAppHelpers
    {
        private const int MAX_PATH = 260;

        public static string AdaptPath(string path)
        {
            if (path.Length < MAX_PATH)
                return path;

            int delimiterIndex = path.LastIndexOf("\\");

            if(delimiterIndex == -1)
                return null;

            string dir = path.Substring(0, delimiterIndex);
            string file = path.Substring(delimiterIndex + 1);

            int fileLen = MAX_PATH - dir.Length - 2;

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
                image.Save(filestream, ImageFormat.Jpeg);
        }

        public static void SaveFile(BitmapImage image, string path)
        {
            if (path == null)
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var filestream = new FileStream(path, FileMode.Create))
                encoder.Save(filestream);
        }

        public static void DragCopyFile(DependencyObject source, string path)
        {
            if (path != null)
            {
                DataObject dragObject = new DataObject();
                dragObject.SetFileDropList(new System.Collections.Specialized.StringCollection() { path });
                DragDrop.DoDragDrop(source, dragObject, DragDropEffects.Copy);
            }
        }
    }
}
