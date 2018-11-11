using System.Windows;

namespace FaceApp
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                //Directory.Delete(Helpers.TempFolderPath, true);
            }
            catch
            {
            }
        }
    }
}