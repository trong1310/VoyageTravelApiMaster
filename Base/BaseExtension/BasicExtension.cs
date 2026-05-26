using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using TravelMasterApi.Base.Pages;

namespace TravelMasterApi.Base.BaseExtension
{
    public static class BasicExtension
    {
        public static string AppendToURL(this string baseURL, params string[] segments)
        {
            return string.Join("/", new[] { baseURL.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/'))));
        }
        public static async Task<string> GetBodyStreamString(this Stream bodyStream)
        {
            using var reader = new StreamReader(
                bodyStream,
                Encoding.UTF8,
                false,
                512, true);

            var requestBody = await reader.ReadToEndAsync();
            bodyStream.Position = 0;
            return requestBody;
        }
        public static async Task<PagedList<T>> TakePage<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            return await PagedList<T>.ToPagedList(source, pageNumber, pageSize);
        }

        public static PagedList<T> TakePage<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            return PagedList<T>.ToPagedList(source, pageNumber, pageSize);
        }

        public static T ToJsonObject<T>(this string str, bool try_catch = true) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str, JSON_SETTING);
            }
            catch (Exception ex)
            {
                // ignored
            }

            if (!try_catch)
                throw new Exception();

            return null;
        }

        public static string ToJsonString(this object obj)
        {
            return obj == null ? "" : JsonConvert.SerializeObject(obj, JSON_SETTING);
        }

        public static JsonSerializerSettings JSON_SETTING { get; } = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ",
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };
    }
}
