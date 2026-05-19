using Newtonsoft.Json;
using TravelMasterApi.Base.BaseMessages;
using TravelMasterApi.Base.Utils;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Base.Extensions
{
    public static class SupportExtension
    {
        public static T GetMessage<T>(this T resp, ErrorCode errorCode) where T : BaseResponseMessage
        {
            resp.error.Code = errorCode;

            resp.error.Message = errorCode.ToDescriptionString();

            return resp;
        }

        public static string ToJsonString<T>(this T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static string GetTokenByHeader(this HttpRequest request)
        {
            if (request.Headers.ContainsKey("Authorization") &&
            request.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = request.Headers["Authorization"][0]
                    .Substring("Bearer ".Length);

                return token;
            }

            return null;
        }
    }
}
