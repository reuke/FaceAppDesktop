using RestSharp;
using System.Linq;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;
using FaceApp.ViewModels;

namespace FaceApp.Client
{
    public static class FaceAppClient
    {
        private static RestClient client = new RestClient("http://dev.faceapp.io/api/v2.7/photos") { UserAgent = "FaceApp/1.0.229 (Linux; Android 4.4)" };

        private static string ClientID = FaceAppClientService.RandomString(8);
        
        public static async void RequestFilterStates(FaceStateViewModel parentState)
        {
            if (parentState.Error == true)
                foreach (FaceStateViewModel state in parentState.FilterStates)
                {
                    state.Loading = false;
                    state.Error = true;
                }
            
            parentState.Code = await Task.Run(() =>
            {
                RestRequest request = new RestRequest("", Method.POST);
                AddHeaders(request);

                try
                {
                    request.AddFile("file", parentState.FilePath, "rb");
                }
                catch
                {
                    return null;
                }

                string code = client.Execute<UploadResponse>(request).Data.Code;
                
                return code;
            });

            foreach (FaceStateViewModel state in parentState.FilterStates)
                RequestLoad(state);
        }

        public static async void RequestLoad(FaceStateViewModel state)
        {
            BitmapImage image = await Task.Run(() =>
            {
                return GetFiterImage(state.ParentState?.Code, state.Type);
            });

            state.Loading = false;
            state.Image = image;
            state.Error = image == null;
        }
        
        private static BitmapImage GetFiterImage(string code, string filter)
        {
            if (code == null || !FaceAppProperties.Filters.Contains(filter))
                return null;

            string crop = FaceAppProperties.NonCropFilters.Contains(filter)  ? "false" : "true";

            RestRequest request = new RestRequest(code + "/filters/" + filter + "?cropped=" + crop, Method.GET);
            AddHeaders(request);

            int retry = 0;

            IRestResponse response = client.Execute(request);

            while (response.RawBytes.Length == 0 && retry < FaceAppProperties.RetryCount)
            {
                System.Threading.Thread.Sleep(FaceAppProperties.RetryWait);
                response = client.Execute(request);
                retry++;
            }

            BitmapImage image = response.RawBytes.ToBitmapImage();
            image.Freeze();
            return image;
        }

        private static void AddHeaders(RestRequest request)
        {
            request.AddHeader("Accept", "*/*");
            request.AddHeader("X-FaceApp-DeviceID", ClientID);
        }

    }
}
