using TravelMasterApi.Base.BaseExtension;
using TravelMasterApi.Configurations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace TravelMasterApi.Extensions
{
    public static class SupportTransferApiExtensions
    {
        private static readonly string _mediaType = "application/json";
        private static readonly Encoding _encoding = Encoding.UTF8;
        private static readonly string _scheme = "Bearer";
        internal static HttpClient httpClient { get; } = new HttpClient();
        internal static string GetToken(this HttpRequest request)
        {
            string token = string.Empty;
            string authHeader = request.Headers["Authorization"];
            //string t0 = request.Headers.Authorization;
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith(_scheme))
            {
                token = authHeader.Split(" ")[1];
                if (authHeader.Split(" ").Length < 2)
                    token = authHeader;
            }
            return token;
        }
        internal static async Task<T> RequestPostAsync<T>(this BaseUrl url, object data, string token = null) where T : class
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Content = new StringContent(data.ToJsonString(), _encoding, _mediaType);
                httpRequest.RequestUri = new Uri(url.ToString());
                httpRequest.Method = HttpMethod.Post;

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue(_scheme, token);
                }

                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var resp = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

                if (!resp.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failed with status code: {resp.StatusCode}");
                }

                var result = await resp.Content.ReadAsStringAsync();

                return result.ToJsonObject<T>(false);
            }
        }

        internal static async Task<T> RequestGetAsync<T>(this BaseUrl url, List<KeyValuePair<string, string>> parameters, string token = null) where T : class
        {
            var query = string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            var urlWithParams = $"{url}?{query}";

            try
            {
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("token", token);
                    }

                    var response = await client.GetAsync(urlWithParams);
                    var result = await response.Content.ReadAsStringAsync();
                    return result.ToJsonObject<T>(false);
                }
            }
            catch(Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }
        /// <summary>
        /// Tranfer TS
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static async Task<T> RequestGetAsyncTS<T>(this BaseUrl url, string token = null, object data = null) where T : class
        {
            var urlStr = url.ToString();
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                if (data != null)
                {
                    // Convert object data to query string
                    var properties = from p in data.GetType().GetProperties()
                                     where p.GetValue(data, null) != null
                                     select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString());

                    var query = string.Join("&", properties.ToArray());
                    urlStr += "?" + query;
                }

                httpRequest.RequestUri = new Uri(urlStr);
                httpRequest.Method = HttpMethod.Get;
                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue(_scheme, token);
                }

                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var resp = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                var result = await resp.Content.ReadAsStringAsync();
                return result.ToJsonObject<T>(false);
            }
        }
    }
}
