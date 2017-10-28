using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceApp.Client
{
    public class UploadResponse
    {
        [DeserializeAs(Name = "code")]
        public string Code { get; set; }

        [DeserializeAs(Name = "faces")]
        public List<FacePosition> Faces { get; set; }

        [DeserializeAs(Name = "filters")]
        public List<FilterDescription> Filters { get; set; }

    }

    public class FacePosition
    {
        public string id { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int right { get; set; }
        public int bottom { get; set; }
    }

    public class FilterDescription
    {
        public string id { get; set; }
        public string title { get; set; }
        public string group { get; set; }
        public bool is_paid { get; set; }
        public bool only_cropped { get; set; }
        public string matching_paid_filter_id { get; set; }
    }
}
