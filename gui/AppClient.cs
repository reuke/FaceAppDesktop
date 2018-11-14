using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using FaceApp.ViewModels;
using FaceAppApi;

namespace FaceApp
{
    public static class AppClient
    {
        public static IFaceAppClient Client;

        public static List<string> GetFiltersNames()
        {
            return Client.GetAvailableFilters();
        }

        public static async void UploadParentState(FaceStateViewModel parentState)
        {
            parentState.Code = await Task.Run(() => Client.UploadImage(parentState.FilePath));

            if (parentState.Code == null)
                parentState.Error = true;

            parentState.Uploaded = true;
        }

        public static async void RequestLoad(FaceStateViewModel state)
        {
            var image = await Task.Run(() => Client.GetFiterImage(state.ParentState?.Code, state.Type));

            state.Loading = false;
            state.Loaded = true;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                state.Image = image;
                state.Error = image == null;
            });
        }
    }
}