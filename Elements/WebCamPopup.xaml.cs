using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FaceApp.Elements
{
    /// <summary>
    /// Interaction logic for WebCamPopup.xaml
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

        public class ImageTakenEventArgs : EventArgs
        {
            private string _imagePath;

            public ImageTakenEventArgs(string ImagePath)
            {
                _imagePath = ImagePath;
            }

            public string ImagePath
            {
                get { return _imagePath; }
            }
        }
        
        private void TakePicture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string imagePath = Path.Combine(FaceAppProperties.TempFolderPath, "WebCam", "webcam_" + DateTime.Now.ToString("MMddyyHHmmss") + ".jpg");
                FaceAppHelpers.SaveFile(WebCam.CurrentBitmap, imagePath);
                PictureTaken(this, new ImageTakenEventArgs(imagePath));
            }
            catch { }
            Visibility = Visibility.Hidden;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WebCamSelector.SelectedIndex = 0;
        }
    }
}
