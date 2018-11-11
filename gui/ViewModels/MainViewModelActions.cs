using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Win32;
using static FaceApp.Elements.WebCamPopup;

namespace FaceApp.ViewModels
{
    public partial class MainViewModel : Screen
    {
        public void OpenImage()
        {
            var dlg = new OpenFileDialog
            {
                Filter =
                    "Image Files(*.png; *.jpg; *.bmp; *.gif)| *.png; *.bmp; *.jpg; *.jpeg; *.gif | All files(*.*) | *.*"
            };


            if (dlg.ShowDialog() == true) HandleFileOpen(dlg.FileName);
        }

        public void WebCamPictureTaken(ImageTakenEventArgs e)
        {
            if (e != null) HandleFileOpen(e.ImagePath);
        }

        public void FileDropped(DragEventArgs e)
        {
            if (e != null)
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                    if (files != null)
                    {
                        var fileName = files[0];
                        if (!fileName.Contains(Helpers.TempFolderPath))
                            HandleFileOpen(files[0]);
                    }
                }
        }

        private void HandleFileOpen(string fileName)
        {
            try
            {
                MainState = new FaceStateViewModel(fileName);
            }
            catch
            {
                // ignored
            }
        }

        public void SaveImage()
        {
            var dlg = new SaveFileDialog
            {
                FileName = PreviewState.FileName + ".jpg",
                Filter = "JPEG Image(*.jpg; *.jpeg;)| *.jpg; *.jpeg;"
            };
            
            if (dlg.ShowDialog() == true)
                Helpers.SaveFile(PreviewState.Image, dlg.FileName);
        }

        public void FaceStateSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                if (e.AddedItems[0] is FaceStateViewModel model)
                    if (!model.Loading && !model.Error)
                        PreviewState = model;
            }
        }

        public void FaceStateDoubleClick(MouseButtonEventArgs e)
        {
            if ((e.Source as ListView)?.SelectedItem is FaceStateViewModel model && model != MainState)
                if (!model.Loading && !model.Error)
                    MainState = model;
        }
    }
}