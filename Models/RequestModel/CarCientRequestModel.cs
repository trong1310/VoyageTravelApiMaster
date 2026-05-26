using TravelMasterApi.Base.BaseMessages;

namespace TravelMasterApi.Models.RequestModel
{
    public class CarCientRequestModel : BaseRequestMessage
    {
        public ulong? IsHot { get; set; }
    }
}
