using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DemoRequestSenderApp.Model
{
    public class RequestContent
    {
        public Dictionary<string, string[]> Headers { get; set; }
        public string Method { get; set; }
        public string Body { get; set; }
        public bool HasBody => !string.IsNullOrEmpty(Body);
        public bool HasHeaders => Headers != null && Headers.Any();
    }
}
