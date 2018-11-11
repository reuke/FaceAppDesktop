using System.Windows;
using System.Windows.Controls;

namespace FaceApp.Views
{
    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void WebCamShow_Click(object sender, RoutedEventArgs e)
        {
            WebCam.Visibility = Visibility.Visible;
        }

        private void FilterStates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FaceStatesHistory.SelectedIndex = -1;
        }

        private void FaceStatesHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterStates.SelectedIndex = -1;
        }
    }
}