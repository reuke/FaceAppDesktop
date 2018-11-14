namespace FaceAppApi.V3.Json
{
    public class RegistrationRequest
    {
        public string app_version { get; set; } = "3.2.1";
        public string device_id { get; set; } = "6c71d977131d0520";
        public string device_model { get; set; } = "ZTE U950";
        public string lang_code { get; set; } = "en-US";
        public string registration_id { get; set; } = Helper.RandomString(11) + ":" + Helper.RandomString(140);
        public string sandbox { get; set; } = "False";
        public string system_version { get; set; } = "4.4.2";
        public string token_type { get; set; } = "fcm";
    }
}