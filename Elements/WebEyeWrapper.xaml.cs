using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Windows.Forms.Layout;
using WebEye.Controls.Wpf;
using System.ComponentModel;
using System.Drawing;

namespace FaceApp.Elements
{
    /// <summary>
    /// Interaction logic for WebEyeWrapper.xaml
    /// </summary>
    public partial class WebEyeWrapper : UserControl
    {
        public WebEyeWrapper()
        {
            InitializeComponent();
        }

        public IEnumerable<WebCameraId> WebCamDevices => WebEyeControl.GetVideoCaptureDevices();
        public IEnumerable<string> WebCamDevicesNames => WebCamDevices.Select(w => w.Name);

        WebCameraId _selectedWebCamDevice = null;
        public WebCameraId SelectedWebCamDevice
        {
            get
            {
                return _selectedWebCamDevice;
            }
            set
            {
                _selectedWebCamDevice = value;
                CurrentWebCamDevice = value;
            }
        }

        WebCameraId _currentWebCamDevice = null;
        private WebCameraId CurrentWebCamDevice
        {
            get
            {
                return WebEyeControl.IsCapturing? _currentWebCamDevice : null;
            }
            set
            {
                if (WebEyeControl.IsCapturing)
                    WebEyeControl.StopCapture();
                _currentWebCamDevice = value;
                if (value != null)
                    WebEyeControl.StartCapture(value);
            }
        }

        public string SelectedWebCamDeviceName
        {
            get
            {
                return SelectedWebCamDevice?.Name;
            }
            set
            {
                SelectedWebCamDevice = WebCamDevices.FirstOrDefault(w => w.Name == value);
            }
        }

        public Bitmap CurrentBitmap => WebEyeControl.GetCurrentImage();

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CurrentWebCamDevice = (bool)e.NewValue ? SelectedWebCamDevice : null;
        }
    }
}
