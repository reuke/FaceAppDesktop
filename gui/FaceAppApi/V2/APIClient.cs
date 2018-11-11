using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using FaceApp.Client;
using RestSharp;

namespace FaceAppApi.V2
{
    public class ApiClient : IFaceAppClient
    {
        private static readonly string ClientId = Helper.RandomString(8);

        private readonly RestClient client = new RestClient("http://node-03.faceapp.io/api/v2.7/photos")
            {UserAgent = "FaceApp/2.0.957 (Linux; Android 4.4)"};

        public List<string> GetAvailableFilters()
        {
            return Properties.Filters.ToList();
        }

        public string UploadImage(string filePath)
        {
            var request = new RestRequest("", Method.POST);
            AddHeaders(request);

            try
            {
                request.AddFile("file", filePath, "rb");
            }
            catch
            {
                return null;
            }

            var code = client.Execute<UploadResponse>(request).Data.Code;

            return code;
        }

        public BitmapImage GetFiterImage(string code, string filter)
        {
            if (code == null || !Properties.Filters.Contains(filter))
                return null;

            var crop = Properties.NonCropFilters.Contains(filter) ? "false" : "true";

            var request = new RestRequest(code + "/filters/" + filter + "?cropped=" + crop, Method.GET);
            AddHeaders(request);

            var retry = 0;

            var response = client.Execute(request);

            while (response.RawBytes.Length == 0 && retry < Properties.RetryCount)
            {
                Thread.Sleep(Properties.RetryWait);
                response = client.Execute(request);
                retry++;
            }

            var image = response.RawBytes.ToBitmapImage();
            image.Freeze();
            return image;
        }

        private static void AddHeaders(RestRequest request)
        {
            request.AddHeader("Accept", "*/*");
            request.AddHeader("X-FaceApp-DeviceID", ClientId);
        }
    }
}