using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using FaceAppApi.V3.Json;
using RestSharp;

namespace FaceAppApi.V3
{
    public class ApiClient : IFaceAppClient
    {
        private static readonly string AppLaunched = Helper.RandomNumberString(10);
        private static readonly string DeviceId = Helper.RandomString(16);
        private static readonly string UserAgent = "FaceApp/3.2.1 (Linux; Android 4.4)";

        private RestClient client;

        public List<string> GetAvailableFilters()
        {
            return Properties.Filters.ToList();
        }

        public ApiClient()
        {
            RegisterClient();
            GetHost();
        }

        private void RegisterClient()
        {
            var registrationClient = new RestClient("https://api.faceapp.io")
                { UserAgent = UserAgent };

            var request = new RestRequest("/api/v3.0/devices/register", Method.POST);
            
            AddHeaders(request);

            request.AddJsonBody(new RegistrationRequest());
            
            registrationClient.Execute(request);
        }

        private void GetHost()
        {
            var hostClient = new RestClient("https://hosts.faceapp.io")
                {UserAgent = UserAgent};

            var request = new RestRequest("/v3/hosts.json", Method.GET);

            AddHeaders(request);

            request.AddJsonBody(new RegistrationRequest());

            var response = hostClient
                .Execute<List<HostsResponse>>(request)
                .Data
                .FirstOrDefault();

            if (response == null)
                client = null;
            else
                client = new RestClient($"https://{response.host}:{response.port}/api/v3.1/photos")
                { UserAgent = UserAgent};

        }

        public string UploadImage(string filePath)
        {
            if (client == null || 
                String.IsNullOrEmpty(filePath))
                return null;

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

            var response = client.Execute<UploadResponse>(request);

            var error = response.Headers.FirstOrDefault(t => t.Name == "X-FaceApp-ErrorCode");
            if (error != null)
                OnErrorOccurred($"Error: {error.Value}");
            
            return response.Data?.Code;
        }

        public BitmapSource GetFiterImage(string code, string filter)
        {
            if (client == null ||
                String.IsNullOrEmpty(code) ||
                String.IsNullOrEmpty(filter) ||
                !Properties.Filters.Contains(filter))
                return null;

            var request = new RestRequest(code + "/filters/" + filter);
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
            image = Properties.NeedToCrop.Contains(filter) ? Helper.CropBitmapImage(image) : image;
            image.Freeze();

            return image;
        }

        public event EventHandler<string> ErrorOccurred;
        public void OnErrorOccurred(string errorStatus) => ErrorOccurred?.Invoke(this, errorStatus);

        private static void AddHeaders(RestRequest request)
        {
            request.AddHeader("Accept", "*/*");
            request.AddHeader("X-FaceApp-AppLaunched", AppLaunched);
            request.AddHeader("X-FaceApp-DeviceID", DeviceId);
        }
    }
}