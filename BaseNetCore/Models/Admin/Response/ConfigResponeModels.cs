namespace TravelMasterApi.Models.Admin.Response
{
    public class GetConfigResponseMessage
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public int Type { get; set; }
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public string? SlugLocation { get; set; }
        /// <summary>
        /// nếu là type = price thì value = tiền còn type = duration thì value = số ngày , số đêm (min = đêm max = ngày)
        /// </summary>
        public double? ValueMin { get; set; }
        public double? ValueMax { get; set; }

    }
    public class GetTypeFilterResponseMessage
    {
        public long? Id { get; set; }
        public string? Name { set; get; }
        public string? Code { get; set; }
    }
    public class TopicGetResponseMessage
    {
        public string? Slug { get; set; }
        public string? Title { set; get; }
        public string? Images { set; get; }
        public long? Categories { get; set; }
        public List<GetTopicItems>? Items { get; set; }
    }
    public class GetTopicItems
    {
        public string? Slug { get; set; }
        public string? Name { set; get; }
    }
    public class GetTypeResponseMessage
    {
        public long? Id { get; set; }
        public string? Name { set; get; }
    }
    public class GetLocationsResponseMessage
    {
        public string? Name { set; get; }
        public string? Slug { get; set; }
        public string? Country { get; set; }
        public string? SlugCountry { get; set; }
        public sbyte? Type { get; set; }
        public ulong? IsCountry { get; set; }
        public string? Image { get; set; }
        
    }
    public class GetBanerResponseMessage
    {
        public long? Id { get; set; }
        public string? Images { get; set; }
        public long? CategoryId { get; set; }
        public sbyte? Position { get; set; }
        public string? CategoryName { get; set; }
        public string? Url { get; set; }
    }
    public class GetEmailSettingResponseMessage
    {
        public string? Uuid { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
