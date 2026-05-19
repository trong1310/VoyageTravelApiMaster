using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class GetConfigRequestModels : BaseRequestMessageKeyword
    {
        public long? Type { get; set; }
        public long? CategoryId { get; set; }
    }
    public class CreateConfigRequestModels
    {
        public long? CategoryId { get; set; }
        public List<CreateFilterObjectsRequestModels>? ListType { get; set; }


    }
    public class CreateFilterObjectsRequestModels
    {
        public sbyte? Type { get; set; }
        public List<ConfigFilterObject>? Items { get; set; }
    }
    public class ConfigFilterObject
    {
        public string? Name { get; set; }
        public string? SlugLocation { get; set; }
        /// <summary>
        /// nếu là type = price thì value = tiền còn type = duration thì value = số ngày , số đêm (min = đêm max = ngày)
        /// </summary>
        public double? ValueMin { get; set; }
        public double? ValueMax { get; set; }
    }
    public class UpdateConfigRequestModels
    {
        public long? Id { get; set; }
        public sbyte? Type { get; set; }
        public long? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? SlugLocation { get; set; }
        /// <summary>
        /// nếu là type = price thì value = tiền còn type = duration thì value = số ngày , số đêm (min = đêm max = ngày)
        /// </summary>
        public double? ValueMin { get; set; }
        public double? ValueMax { get; set; }

    }
    public class DeleteConfigFilterRequestModels
    {
        public List<long>? Id { get; set; }
    }
    public class GetTopicRequestModels : BaseRequestMessageKeyword
    {
        public sbyte? Categories { get; set; }
    }
    public class CreateTopicRequestModels
    {
        public string? Name { get; set; }
        public string? Images { get; set; }
        public long? CategoryId { get; set; }
        public List<ReferenceTopicRequestModels>? ReferenceTopics { get; set; }
    }
    public class UpdateTopicRequestModels : CreateTopicRequestModels
    {
        public string? Slug { get; set; }
    }
    public class ReferenceTopicRequestModels
    {
        public string Slug { get; set; }
    }
    public class CreateLocationRequestModels
    {
        public string? Name { get; set; }
        public string? SlugCountry { get; set; }
        public sbyte? Type { get; set; }
        public ulong IsCountry { get; set; }
        public string? ImageUrl { get; set; }

    }
    public class UpdateLocationRequestModels : CreateLocationRequestModels
    {
        public string? Slug { get; set; }
    }
    public class GetTypeRequestModels : BaseRequestMessageKeyword
    {
        public sbyte? Category { get; set; }
    }
    public class LocationRequestModels : BaseRequestMessageKeyword
    {
        public sbyte? Type { get; set; }
        public ulong? IsCountry { get; set; }
    }
    public class UpdateBanerRequestModels
    {
        public long? Id { get; set; }
        public long? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public sbyte? Position { get; set; }
        public string? Url { get; set; }
    }
    public class CreateBanerRequestModels
    {
        public long? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public sbyte? Position { get; set; }
        public string? Url { get; set; }
    }
    public class GetBanerRequestModels
    {
        public sbyte? CategoryId { get; set; }
    }
    public class CreateEmailSettingRequestModels
    {
        public string? Email { get; set; }
    }
    public class UpdateEmailSettingRequestModels
    {
        public string? Uuid { get; set; }
        public string? Email { get; set; }
    }
}