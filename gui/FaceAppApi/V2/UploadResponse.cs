using System.Collections.Generic;
using RestSharp.Deserializers;

namespace FaceAppApi.V2
{
    public class UploadResponse
    {
        [DeserializeAs(Name = "code")] public string Code { get; set; }

        [DeserializeAs(Name = "faces")] public List<FacePosition> Faces { get; set; }

        [DeserializeAs(Name = "filters")] public List<FilterDescription> Filters { get; set; }
    }

    public class FacePosition
    {
        public string Id { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }

    public class FilterDescription
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public bool IsPaid { get; set; }
        public bool OnlyCropped { get; set; }
        public string MatchingPaidFilterId { get; set; }
    }
}