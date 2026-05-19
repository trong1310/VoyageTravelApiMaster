using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.Admin.Request
{
    public class GetBookingManagerRequestModels : BaseRequestMessageKeywordTime
    {
        public long? CategoryId { get; set; }
        public sbyte? State { get; set; }
    }
    public class DetailBookingManagerRequestModels
    {
        public string? Uuid { get; set; }
    }
    public class UpdateStateBookingRequestModels
    {
        public string? Uuid { get; set; }
        public sbyte State { get; set; }
    }
}
