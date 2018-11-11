using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace FaceApp.ViewModels
{
    public partial class MainViewModel : Screen
    {
        private FaceStateViewModel mainState;

        private FaceStateViewModel previewState;

        public MainViewModel()
        {
            MainState = new FaceStateViewModel();
            DisplayName = "FaceApp Desktop";
        }

        public bool IsEmpty => MainState.Type == null;

        public FaceStateViewModel PreviewState
        {
            get => previewState;
            set
            {
                previewState = value;
                NotifyOfPropertyChange(() => PreviewState);
            }
        }

        public FaceStateViewModel MainState
        {
            get => mainState;
            set
            {
                mainState = value;
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
                var history = new ObservableCollection<FaceStateViewModel>();

                var currentFaceStateModel = MainState;

                while (currentFaceStateModel?.Type != null)
                {
                    history.Add(currentFaceStateModel);
                    currentFaceStateModel = currentFaceStateModel.ParentState;
                }

                return history;
            }
        }
    }
}