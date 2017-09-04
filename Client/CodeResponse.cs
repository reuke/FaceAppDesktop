using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceApp.Client
{
    class CodeResponse
    {
        [DeserializeAs(Name = "code")]
        public string Code{ get; set; }
    }
}
