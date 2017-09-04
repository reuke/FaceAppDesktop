using System;
using System.IO;

namespace FaceApp
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        private void Application_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            try
            {
                Directory.Delete(FaceAppProperties.TempFolderPath, true);
            }
            catch { }
        }
    }
}
