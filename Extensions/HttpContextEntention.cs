namespace TravelMasterApi.Extensions
{
    public static class HttpContextEntention
    {
        public static string? GetToken(this HttpContext context)
        {
            // Lấy giá trị của Authorization header từ yêu cầu HTTP
            string authorizationHeader = context.Request.Headers["Authorization"];
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }    

            return null;
        }
    }
}
