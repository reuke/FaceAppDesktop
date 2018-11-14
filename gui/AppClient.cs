using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using FaceApp.ViewModels;
using ApiClient = FaceAppApi.V2.ApiClient;

namespace FaceApp
{
    public static class AppClient
    {
        private static readonly ApiClient Client = new ApiClient();

        public static List<string> GetFiltersNames()
        {
            return Client.GetAvailableFilters();
        }

        public static async void RequestFilterStates(FaceStateViewModel parentState)
        {
            if (parentState.Error)
                foreach (var state in parentState.FilterStates)
                {
                    state.Loading = false;
                    state.Error = true;
                }

            parentState.Code = await Task.Run(() => Client.UploadImage(parentState.FilePath));

            foreach (var state in parentState.FilterStates)
                RequestLoad(state);
        }

        public static async void RequestLoad(FaceStateViewModel state)
        {
            var image = await Task.Run(() => Client.GetFiterImage(state.ParentState?.Code, state.Type));

            state.Loading = false;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                state.Image = image;
                state.Error = image == null;
            });
        }
    }
}