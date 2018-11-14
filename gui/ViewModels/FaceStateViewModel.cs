using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace FaceApp.ViewModels
{
    public class FaceStateViewModel : Screen
    {
        private ObservableCollection<FaceStateViewModel> filterStates;

        private string id;

        private BitmapSource image;
        
        private string type;

        public bool Loaded;
        
        public bool Filtered;

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
                Loaded = true;
                Filtered = false;
                Uploaded = false;
                Paused = false;
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
            Loaded = true;
            Filtered = false;
            Uploaded = false;
            Paused = false;
        }

        public FaceStateViewModel(FaceStateViewModel parent, string type)
        {
            ParentState = parent;
            Type = type;
            Loaded = false;
            Filtered = true;
            Uploaded = false;

            if (SettingManager.PreLoad)
                Loading = true;
            else
                Paused = true;
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

                    AppClient.UploadParentState(this);

                    if (SettingManager.PreLoad)
                        LoadAllChildren();
                }

                return filterStates;
            }
        }

        public void LoadAllChildren()
        {
            Task task = Task.Run(() =>
            {
                SpinWait.SpinUntil(() => Uploaded, 30000);

                if (!Uploaded)
                    Error = true;
                    
                if (Error)
                    return;

                foreach (var filterState in filterStates)
                    filterState.LoadFilteredState();
            });
        }

        public void LoadFilteredState()
        {
            Task task = Task.Run(() =>
            {
                SpinWait.SpinUntil(() => ParentState.Uploaded, 30000);

                if (!ParentState.Uploaded)
                    Error = true;

                if (Loaded ||
                    !Filtered ||
                    Type == "origin" ||
                    String.IsNullOrEmpty(Type) ||
                    Error)
                    return;

                Loading = true;
                Paused = false;
                AppClient.RequestLoad(this);
            });
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

        private bool loading;
        public bool Loading
        {
            get => (loading || (!ParentState?.Uploaded ?? false)) && !Error;
            set
            {
                loading = value;
                NotifyOfPropertyChange(() => Loading);
                NotifyOfPropertyChange(() => Blur);
            }
        }

        public void UpdateLoading()
        {
            NotifyOfPropertyChange(() => Error);
            NotifyOfPropertyChange(() => Paused);
            NotifyOfPropertyChange(() => Loading);
            NotifyOfPropertyChange(() => Blur);
        } 

        private bool uploaded;
        public bool Uploaded
        {
            get => uploaded;
            set
            {
                uploaded = value;

                if (filterStates != null)
                    foreach (var filterState in filterStates)
                        filterState.UpdateLoading();

                NotifyOfPropertyChange(() => Uploaded);
                NotifyOfPropertyChange(() => Loading);
                NotifyOfPropertyChange(() => Blur);
            }
        }

        private bool error;
        public bool Error
        {
            get => error;
            set
            {
                error = value;
                Loading = false;
                Paused = false;
                
                if (filterStates != null && value)
                    foreach (var filterState in filterStates)
                        filterState.Error = true;

                NotifyOfPropertyChange(() => Error);
                NotifyOfPropertyChange(() => Loading);
                NotifyOfPropertyChange(() => Blur);
            }
        }

        private bool paused;
        public bool Paused
        {
            get => paused && !Error && !Loading;
            set
            {
                paused = value;

                NotifyOfPropertyChange(() => Paused);
                NotifyOfPropertyChange(() => Blur);
            }
        }

        public bool Blur => Loading || Error || Paused;

        public void MouseLeaveForDrag(FaceStateViewModel source, MouseEventArgs e)
        {
            if (Loading || Error || Paused || source == null || e == null)
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
                Helpers.DragCopyFile((DependencyObject) e.Source, source.FilePath);
        }
    }
}