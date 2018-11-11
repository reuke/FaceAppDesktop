using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FaceApp.Elements
{
    /// <summary>
    ///     Interaction logic for WebCamPopup.xaml
    /// </summary>
    public partial class WebCamPopup : UserControl
    {
        public WebCamPopup()
        {
            InitializeComponent();
        }

        private void HideThis_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        public event EventHandler<ImageTakenEventArgs> PictureTaken;

        private void TakePicture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var imagePath = Path.Combine(Helpers.TempFolderPath, "WebCam",
                    "webcam_" + DateTime.Now.ToString("MMddyyHHmmss") + ".jpg");
                Helpers.SaveFile(WebCam.CurrentBitmap, imagePath);
                PictureTaken(this, new ImageTakenEventArgs(imagePath));
            }
            catch
            {
                // ignored
            }

            Visibility = Visibility.Hidden;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WebCamSelector.SelectedIndex = 0;
        }

        public class ImageTakenEventArgs : EventArgs
        {
            public ImageTakenEventArgs(string imagePath)
            {
                ImagePath = imagePath;
            }

            public string ImagePath { get; }
        }
    }
}