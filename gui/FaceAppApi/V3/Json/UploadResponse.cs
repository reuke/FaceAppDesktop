using RestSharp.Deserializers;

namespace FaceAppApi.V3.Json
{
    public class UploadResponse
    {
        [DeserializeAs(Name = "code")] public string Code { get; set; }
    }
}