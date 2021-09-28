using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRequestSenderApp.Model
{
    public class ResponseContent
    {
        public string HttpStatusCode { get; set; }
        public string Body { get; set; }
        public string Headers { get; set; }
    }
}
