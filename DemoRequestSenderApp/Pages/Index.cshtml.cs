using DemoRequestSenderApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoRequestSenderApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        [Required]
        [BindProperty]
        public string RequestUrl { get; set; } = "http://";

        [BindProperty]
        public string RequestContent { get; set; }

        [BindProperty]
        public ResponseContent RequestResponse { get; set; }

        public IndexModel()
        {
            _client = new HttpClient();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task OnPostAsync()
        {

            var requestContent = RequestContent != null ? JsonSerializer.Deserialize<RequestContent>(RequestContent, _jsonSerializerOptions) : new RequestContent { Method = "GET" };
            var httpRequestMessage = new HttpRequestMessage() { RequestUri = new Uri(RequestUrl) };
            if (string.IsNullOrEmpty(requestContent.Method))
            {
                requestContent.Method = "GET";
            }
            switch (requestContent.Method.ToUpperInvariant())
            {
                case "POST":
                    httpRequestMessage.Method = HttpMethod.Post;
                    AddBody(requestContent, httpRequestMessage);
                    break;
                case "PUT":
                    httpRequestMessage.Method = HttpMethod.Put;
                    AddBody(requestContent, httpRequestMessage);
                    break;
                case "DELETE":
                    httpRequestMessage.Method = HttpMethod.Delete;
                    break;
                case "GET":
                    httpRequestMessage.Method = HttpMethod.Get;
                    break;

                default: break;
            }
            AddHeaders(requestContent, httpRequestMessage);

            var result = await _client.SendAsync(httpRequestMessage);
            RequestResponse = new ResponseContent { Body = await result.Content.ReadAsStringAsync(), HttpStatusCode = (int)result.StatusCode + " " + result.StatusCode.ToString(), Headers = string.Join(" | ", result.Headers.Select(h => $"{h.Key} : {string.Join(", ", h.Value)}")) };

        }

        private static void AddBody(RequestContent requestContent, HttpRequestMessage httpRequestMessage)
        {
            if (requestContent.HasBody)
            {
                httpRequestMessage.Content = new StringContent(requestContent.Body);
                var contentHeaders = requestContent.Headers.Where(c => c.Key.StartsWith("Content"));
                foreach (var item in contentHeaders)
                {
                    httpRequestMessage.Content.Headers.Add(item.Key, item.Value);
                }

            }
        }

        private static void AddHeaders(RequestContent requestContent, HttpRequestMessage httpRequestMessage)
        {
            if (requestContent.HasHeaders)
            {
                foreach (var item in requestContent.Headers)
                {
                    if (item.Key.StartsWith("Content"))
                        continue;
                    httpRequestMessage.Headers.Add(item.Key, item.Value);
                }

            }
        }
    }
}
