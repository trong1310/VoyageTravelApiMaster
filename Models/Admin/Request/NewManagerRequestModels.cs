using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class CreateNewManagerRequestModels
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? SeoDescription { get; set; }
        public string? ThumbNail { get; set; }
        public sbyte? Type { get; set; }
        public ulong? IsHot { get; set; }
        public ulong? SendMail { get; set; }
    }

    public class UpdateNewManagerRequestModels
    {
        public string Slug { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ThumbNail { get; set; }
        public sbyte? Type { get; set; }
        public ulong? IsHot { get; set; }
        public string? SeoDescription { get; set; }
    }
    public class GetNewManagerRequestModels : BaseRequestMessageKeyword
    {
        public sbyte? Type { get; set; }
    }
    public class  SendMailRequestModel
    {
        public int? Type { get; set; }
        public string? SlugNew { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public List<string>? ListEmail { get; set; }
        public int? SendAll { get; set; }
    }
}
