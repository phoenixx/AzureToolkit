using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PATK.Domain;
using PATK.Domain.Rest;
using PATK.Rest.Auth;
using PATK.Rest.Extensions;

namespace PATK.Rest.RestConsumer
{
    public class RestConsumer : IRestConsumer
    {
        private readonly string _baseUrl;
        private readonly string _version;
        private readonly string _token;
        private const string JSON_TYPE = "application/json";
        private const string STANDARD_ERROR = "An error occurred. Please check the error property.";
        private static JsonSerializerSettings _serializerSettings;

        public RestConsumer()
        {
            _version = Config.Version;
            _token = AuthToken.GenerateAuthToken();
            _serializerSettings = new JsonSerializerSettings();
        }

        public async Task<ApiResponse<T>> Get<T>(string destination)
        {
            using (var client = CreateHttpClient())
            {
                using (var response = client.GetAsync(BuildQueryString(destination)))
                {
                    var result = await response;
                    var content = await result.Content.ReadAsStringAsync();
                    var converted = await Deserialize<T>(content);
                    return new ApiResponse<T>(result.StatusCode, string.Empty, converted);
                }
            }
        }

        public async Task<ApiResponse<TOut>> Put<T, TOut>(T model, string destination)
        {
            var content = SerializeObject(model);
            using (var client = CreateHttpClient())
            {
                var response = await client.PutAsync(BuildQueryString(destination),
                    new StringContent(content, Encoding.UTF8, JSON_TYPE));

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = await DeserializeError(result);
                    return new ApiResponse<TOut>(response.StatusCode, STANDARD_ERROR, default(TOut), error);
                }

                var converted = await Deserialize<TOut>(result);
                return new ApiResponse<TOut>(response.StatusCode, string.Empty, converted);
            }
        }

        public async Task<ApiResponse<TOut>> Patch<T, TOut>(T model, string destination, Dictionary<string, string> optionalHeaders)
        {
            var content = SerializeObject(model);
            using (var client = CreateHttpClient(optionalHeaders))
            {
                var response = await client.PatchAsync(BuildQueryString(destination),
                    new StringContent(content, Encoding.UTF8, JSON_TYPE));
                var result = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                var converted = await Deserialize<TOut>(result);
                return new ApiResponse<TOut>(response.StatusCode, string.Empty, converted);
            }
        }

        public async Task<ApiResponse<bool>> Delete(string destination, Dictionary<string, string> optionalHeaders = null)
        {
            using (var client = CreateHttpClient(optionalHeaders))
            {
                var response = await client.DeleteAsync(BuildQueryString(destination));
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>(HttpStatusCode.NoContent, "The resource has been deleted", true);
                }
                else
                {
                    var error = await DeserializeError(content);
                    return new ApiResponse<bool>(response.StatusCode, STANDARD_ERROR, false, error);
                }
            }
        }

        private HttpClient CreateHttpClient(Dictionary<string, string> optionalHeaders = null)
        {
            var httpClient = new HttpClient() { BaseAddress = new Uri(_baseUrl) };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", _token);
            httpClient = ApplyOptionalHeaders(httpClient, optionalHeaders);
            return httpClient;
        }

        private string BuildQueryString(string destination)
        {
            const string API_VERSION_PARAM = "api-version=";
            var result = string.Format(
                destination.IndexOf("?", StringComparison.InvariantCulture) > -1
                ? "{0}&{1}{2}"
                : "{0}?{1}{2}", destination, API_VERSION_PARAM, _version);

            return result;
        }

        private static async Task<T> Deserialize<T>(string content)
        {
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(content, _serializerSettings)).ConfigureAwait(false);
        }

        private static async Task<ApiError> DeserializeError(string content)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    return JsonConvert.DeserializeObject<ApiError>(content, _serializerSettings);
                }
                catch (Exception ex)
                {
                    return new ApiError()
                    {
                        Error = new ApiErrorDetails()
                        {
                            Code = "Deserialize",
                            Details = new List<ApiErrorMessage>()
                            {
                                new ApiErrorMessage()
                                {
                                    Code = "Exception",
                                    Message = ex.Message,
                                    Target = string.Empty
                                }
                            }
                        }
                    };
                }
            });
        }

        private static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, _serializerSettings);
        }

        private static HttpClient ApplyOptionalHeaders(HttpClient client, Dictionary<string, string> optionalHeaders)
        {
            if (client == null)
            {
                return null;
            }

            if (optionalHeaders == null)
            {
                return client;
            }

            foreach (var header in optionalHeaders)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            return client;
        }
    }
}