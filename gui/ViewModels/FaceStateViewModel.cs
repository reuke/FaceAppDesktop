using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace FaceApp.ViewModels
{
    public class FaceStateViewModel : Screen
    {
        private bool error;

        private ObservableCollection<FaceStateViewModel> filterStates;

        private string id;

        private BitmapSource image;

        private bool loadingIsVisible;

        private string type;

        public string Code;

        public FaceStateViewModel ParentState;

        public FaceStateViewModel()
        {
            filterStates = new ObservableCollection<FaceStateViewModel>();
            Loading = false;
        }

        public FaceStateViewModel(string imagePath)
        {
            try
            {
                Type = Path.GetFileNameWithoutExtension(imagePath);
                var img = new BitmapImage(new Uri(imagePath));
                img.Freeze();
                Image = img;
                Loading = false;
            }
            catch
            {
                Error = true;
            }
        }

        public FaceStateViewModel(BitmapImage image)
        {
            Type = "origin";
            image.Freeze();
            Image = image;
            Loading = false;
        }

        public FaceStateViewModel(FaceStateViewModel parent, string type)
        {
            ParentState = parent;
            Type = type;
            Loading = true;
        }

        public ObservableCollection<FaceStateViewModel> FilterStates
        {
            get
            {
                if (filterStates == null)
                {
                    filterStates =
                        new ObservableCollection<FaceStateViewModel>(
                            AppClient.GetFiltersNames().Select(f => new FaceStateViewModel(this, f)));
                    AppClient.RequestFilterStates(this);
                }

                return filterStates;
            }
        }

        public string Id => id ?? (id = Guid.NewGuid().ToString());

        public string FileName => ParentState == null ? Type : ParentState.FileName + "_" + Type;

        public string FolderPath => ParentState == null
            ? Path.Combine(Helpers.TempFolderPath, Id)
            : ParentState.FolderPath;

        public string FilePath => Helpers.AdaptPath(Path.Combine(FolderPath, FileName + ".jpg"));

        public BitmapSource Image
        {
            get => ParentState != null && image == null ? ParentState?.Image : image;
            set
            {
                if (value == null)
                    return;

                image = value;
                Helpers.SaveFile(value, FilePath);
                NotifyOfPropertyChange(() => Image);
            }
        }

        public string Type
        {
            get => type;
            set
            {
                type = value;
                NotifyOfPropertyChange(() => Type);
            }
        }

        public bool Loading
        {
            get => loadingIsVisible && !Error;
            set
            {
                loadingIsVisible = value;
                NotifyOfPropertyChange(() => Loading);
                NotifyOfPropertyChange(() => Blur);
            }
        }

        public bool Error
        {
            get => error;
            set
            {
                error = value;
                NotifyOfPropertyChange(() => Error);
                NotifyOfPropertyChange(() => Blur);
            }
        }

        public bool Blur => Loading || Error;

        public void MouseLeaveForDrag(FaceStateViewModel source, MouseEventArgs e)
        {
            if (Loading || Error || source == null || e == null)
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
                Helpers.DragCopyFile((DependencyObject) e.Source, source.FilePath);
        }
    }
}