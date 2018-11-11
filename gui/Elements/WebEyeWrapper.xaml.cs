using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebEye.Controls.Wpf;

namespace FaceApp.Elements
{
    /// <summary>
    ///     Interaction logic for WebEyeWrapper.xaml
    /// </summary>
    public partial class WebEyeWrapper : UserControl
    {
        private WebCameraId currentWebCamDevice;

        private WebCameraId selectedWebCamDevice;

        public WebEyeWrapper()
        {
            InitializeComponent();
        }

        public IEnumerable<WebCameraId> WebCamDevices => WebEyeControl.GetVideoCaptureDevices();
        public IEnumerable<string> WebCamDevicesNames => WebCamDevices.Select(w => w.Name);

        public WebCameraId SelectedWebCamDevice
        {
            get => selectedWebCamDevice;
            set
            {
                selectedWebCamDevice = value;
                CurrentWebCamDevice = value;
            }
        }

        private WebCameraId CurrentWebCamDevice
        {
            get => WebEyeControl.IsCapturing ? currentWebCamDevice : null;
            set
            {
                if (WebEyeControl.IsCapturing)
                    WebEyeControl.StopCapture();
                currentWebCamDevice = value;
                if (value != null)
                    WebEyeControl.StartCapture(value);
            }
        }

        public string SelectedWebCamDeviceName
        {
            get => SelectedWebCamDevice?.Name;
            set { SelectedWebCamDevice = WebCamDevices.FirstOrDefault(w => w.Name == value); }
        }

        public Bitmap CurrentBitmap => WebEyeControl.GetCurrentImage();

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CurrentWebCamDevice = (bool) e.NewValue ? SelectedWebCamDevice : null;
        }
    }
}