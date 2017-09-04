using System;
using Caliburn.Micro;
using FaceApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using FaceApp.Views;
using System.Windows.Input;

namespace FaceApp.ViewModels
{
    public partial class MainViewModel : Screen
    {
        public MainViewModel()
        {
            MainState = new FaceStateViewModel();
            DisplayName = "FaceApp Desktop";
        }

        public bool IsEmpty
        {
            get { return MainState.Type == null; }
        }

        FaceStateViewModel _previewState;
        public FaceStateViewModel PreviewState
        {
            get { return _previewState; }
            set
            {
                _previewState = value;
                NotifyOfPropertyChange(() => PreviewState);
            }
        }

        FaceStateViewModel _mainState;
        public FaceStateViewModel MainState
        {
            get { return _mainState; }
            set
            {
                _mainState = value;
                PreviewState = value;
                NotifyOfPropertyChange(() => MainState);
                NotifyOfPropertyChange(() => IsEmpty);
                NotifyOfPropertyChange(() => FaceStatesHistory);
            }
        }

        public ObservableCollection<FaceStateViewModel> FaceStatesHistory
        {
            get
            {
                ObservableCollection<FaceStateViewModel> _history = new ObservableCollection<FaceStateViewModel>();

                FaceStateViewModel currentFaceStateModel = MainState;

                while (currentFaceStateModel != null && currentFaceStateModel.Type != null)
                {
                    _history.Add(currentFaceStateModel);
                    currentFaceStateModel = currentFaceStateModel.ParentState;
                }

                return _history;
            }
        }
    }
}
