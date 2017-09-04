using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FaceApp.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
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
