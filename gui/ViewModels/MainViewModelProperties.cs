using System.Collections.ObjectModel;
using System.Windows.Threading;
using Caliburn.Micro;
using FaceAppApi;

namespace FaceApp.ViewModels
{
    public partial class MainViewModel : Screen
    {
        private FaceStateViewModel mainState;

        private FaceStateViewModel previewState;

        private readonly IFaceAppClient v2Client = new FaceAppApi.V2.ApiClient();
        private readonly IFaceAppClient v3Client = new FaceAppApi.V3.ApiClient();

        public MainViewModel()
        {
            MainState = new FaceStateViewModel();
            DisplayName = "FaceApp Desktop";

            v2Client.ErrorOccurred += (sender, s) =>
                Dispatcher.CurrentDispatcher.Invoke(() =>
                    Status = s);

            v3Client.ErrorOccurred += (sender, s) =>
                Dispatcher.CurrentDispatcher.Invoke(() =>
                    Status = s);

            AppClient.Client = 
                SettingManager.ApiVersion == 2 ? v2Client : v3Client;
        }

        public bool IsEmpty => MainState?.Type == null;

        public FaceStateViewModel PreviewState
        {
            get => previewState;
            set
            {
                previewState = value;
                if (!SettingManager.PreLoad &&
                    previewState.Filtered)
                    previewState.LoadFilteredState();
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
                Status = "";
                if (SettingManager.PreLoad)
                    mainState.LoadAllChildren();
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

        public string ApiSwitchCaption =>
            SettingManager.ApiVersion == 3 ? "API V3" :
            SettingManager.ApiVersion == 2 ? "API V2" :
            "";

        private string status = "";
        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public bool PreLoad
        {
            get => SettingManager.PreLoad;
            set
            {
                SettingManager.PreLoad = value;
                if (SettingManager.PreLoad)
                    mainState.LoadAllChildren();
            }
        }
    }
}