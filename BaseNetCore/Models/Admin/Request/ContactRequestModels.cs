using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class GetContactRequestModels : BaseRequestMessageKeywordTime
    {
        public sbyte? Type { get; set; }
        public sbyte? Status { get; set; }
    }
    public class UpdateContactStatusRequestModels
    {
        public string? Uuid { get; set; }
        public sbyte? Status { get; set; }
    }
}
