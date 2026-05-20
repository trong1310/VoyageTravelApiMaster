using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.RequestModel
{
    public class HotelClientRequestModel : BaseRequestMessage
    {
        public ulong? IsHot { get; set; }
        public int? Ranking { get; set; }
        public sbyte? Type { get; set; }
        public List<string>? Locations { get; set; }
    }
}
