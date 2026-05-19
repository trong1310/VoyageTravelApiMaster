using TravelMasterApi.Models.Admin.Response;

namespace TravelMasterApi.Models.ResponseModel
{
    public class GetClientCarManagerRespones
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Brand { get; set; }
        internal DateTime? CreatedAt { get; set; }
        public string? ThumbNail { get; set; }
        public List<CarRouteResponseObject>? Routes { get; set; }
    }
}
