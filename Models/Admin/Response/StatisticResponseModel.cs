namespace TravelMasterApi.Models.Admin.Response
{
    public class StatisticResponseModel
    {
        public int TotalActiveTours { get; set; }
        public int TotalActiveCars { get; set; }
        public int TotalActiveHotels { get; set; }
        public int TotalSoldTours { get; set; }
        public int TotalCustomerCars { get; set; }
        public int TotalSoldHotels { get; set; }
    }
}
