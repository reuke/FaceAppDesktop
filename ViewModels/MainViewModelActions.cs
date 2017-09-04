using System;
using Caliburn.Micro;
using FaceApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using FaceApp.Views;
using System.Windows.Input;
using Microsoft.Win32;
using static FaceApp.Elements.WebCamPopup;
using System.Windows;

namespace FaceApp.ViewModels
{
    public partial class MainViewModel : Screen
    {
        public void OpenImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Image Files(*.png; *.jpg; *.bmp; *.gif)| *.png; *.bmp; *.jpg; *.jpeg; *.gif | All files(*.*) | *.*";

            if (dlg.ShowDialog() == true)
            {
                HandleFileOpen(dlg.FileName);
            }
        }

        public void WebCamPictureTaken(ImageTakenEventArgs e)
        {
            if (e != null)
            {
                HandleFileOpen(e.ImagePath);
            }
        }

        public void FileDropped(DragEventArgs e)
        {
            if(e != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string fileName = files[0];
                    if (!fileName.Contains(FaceAppProperties.TempFolderPath))
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
            catch { }
        }

        public void SaveImage()
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.FileName = PreviewState.FileName + ".jpg";
            dlg.Filter = "JPEG Image(*.jpg; *.jpeg;)| *.jpg; *.jpeg;";

            if (dlg.ShowDialog() == true)
                FaceAppHelpers.SaveFile(PreviewState.Image, dlg.FileName);
        }
        
        public void FaceStateSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                FaceStateViewModel model = e.AddedItems[0] as FaceStateViewModel;
                if (model != null)
                {
                    if (!model.Loading && !model.Error)
                    {
                        PreviewState = model;
                    }
                }
            }
        }

        public void FaceStateDoubleClick(MouseButtonEventArgs e)
        {
            FaceStateViewModel model = ((e.Source as ListView)?.SelectedItem as FaceStateViewModel);
            if (model != null && model != MainState)
            {
                if (!model.Loading && !model.Error)
                {
                    MainState = model;
                }
            }
        }
    }
}
