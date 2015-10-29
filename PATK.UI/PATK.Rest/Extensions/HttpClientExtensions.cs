using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace PATK.Rest.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri,
            HttpContent iContent)
        {
            var uri = new Uri(requestUri);
            return await PatchAsync(client, uri, iContent);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri,
            HttpContent iContent)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = iContent
            };

            var response = new HttpResponseMessage();
            try
            {
                response = await client.SendAsync(request);
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("Error: " + e.ToString());
            }

            return response;
        }
    }
}