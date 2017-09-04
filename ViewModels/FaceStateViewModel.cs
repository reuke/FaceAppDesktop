using Caliburn.Micro;
using FaceApp.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FaceApp.ViewModels
{
    public class FaceStateViewModel : Screen
    {
        public FaceStateViewModel()
        {
            _filterStates = new ObservableCollection<FaceStateViewModel>();
            Loading = false;
        }

        public FaceStateViewModel(string imagePath)
        {
            try
            {
                Type = Path.GetFileNameWithoutExtension(imagePath);
                BitmapImage image = new BitmapImage(new Uri(imagePath));
                image.Freeze();
                Image = image;
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

        private ObservableCollection<FaceStateViewModel> _filterStates;
        public ObservableCollection<FaceStateViewModel> FilterStates
        {
            get
            {
                if(_filterStates == null)
                {
                    _filterStates = new ObservableCollection<FaceStateViewModel>( FaceAppProperties.Filters.Select(f => new FaceStateViewModel(this, f)));
                    FaceAppClient.RequestFilterStates(this);
                }
                return _filterStates;
            }
        }

        private string _id;
        public string Id
        {
            get
            {
                if (_id == null)
                    _id = Guid.NewGuid().ToString();
                return _id;
            }
        }

        public string Code;

        public string FileName => ParentState == null ? Type : ParentState.FileName + "_" + Type;

        public string FolderPath => ParentState == null ? Path.Combine(FaceAppProperties.TempFolderPath, Id) : ParentState.FolderPath;

        public string FilePath => FaceAppHelpers.AdaptPath(Path.Combine(FolderPath, FileName + ".jpg"));

        public FaceStateViewModel ParentState;

        public void MouseLeaveForDrag(FaceStateViewModel source, MouseEventArgs e)
        {
            if (Loading || Error || source == null || e == null)
                return;
            
            if (e.LeftButton == MouseButtonState.Pressed)
                FaceAppHelpers.DragCopyFile((DependencyObject)e.Source, source.FilePath);
        }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get
            {
                return ParentState != null && _image == null ? ParentState?.Image : _image;
            }
            set
            {
                if (value == null)
                    return;

                _image = value;
                FaceAppHelpers.SaveFile(value, FilePath);
                NotifyOfPropertyChange(() => Image);
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyOfPropertyChange(() => Type);
            }
        }

        private bool _loadingIsVisible;
        public bool Loading
        {
            get { return _loadingIsVisible && !Error; }
            set
            {
                _loadingIsVisible = value;
                NotifyOfPropertyChange(() => Loading);
                NotifyOfPropertyChange(() => Blur);
            }
        }
        
        private bool _error = false;
        public bool Error
        {
            get { return _error; }
            set
            {
                _error = value;
                NotifyOfPropertyChange(() => Error);
                NotifyOfPropertyChange(() => Blur);
            }
        }
        
        public bool Blur
        {
            get { return Loading || Error; }
        }
    }
}
