namespace TravelMasterApi.Models.Admin.Response
{
    public class GetBookingManagerResponeModels
    {
        public string Uuid { get; set; }
        public sbyte? State { get; set; }
        internal sbyte? Type { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ServiceName { get; set; }
        public string? Slug { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class ServiceObject
    {
        public string BookingUuid { get; set; }
        public string? ServiceName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class DetailBookingManagerResponeModels
    {
        public string Uuid { get; set; }
        public sbyte? State { get; set; }
        internal sbyte? Type { get; set; }
        internal string? SlugOwner { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ServiceName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SpecialRequirements { get; set; }
        public string? YourName { get; set; }
        public string? Email { get; set; }
        public string? DepartureName { get; set; }
        public string? Region { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TotalAdults { get; set; } = 0;
        public int TotalChildrens { get; set; } = 0;
        public int TotalBabys { get; set; } = 0;
        /// <summary>
        ///  0: chưa xác định 1: trong nước 2: nước ngoài
        /// </summary>
        public sbyte? TourType { get; set; }
        public List<string>? Locations { get; set; }
        public string? OtherLocation { get; set; }
        public sbyte? TimeOption { get; set; }
        /// <summary>
        /// Other = 0,
        /// Under5M = 1,      // Dưới 5 triệu
        /// From5To10M = 2,   // 5 - 10 triệu
        /// From10To20M = 3,  // 10 - 20 triệu
        /// Over20M = 4       // Trên 20 triệu
        /// </summary>
        public sbyte? Budget { get; set; }
        /// <summary>
        /// Vacation = 1,    // nghỉ dưỡng
        /// ExploreAndExperience = 2,   // Khám phá trải nghiệm
        /// Collaborate = 3,  //Công tác
        /// Spirituality = 4       // Tâm linh
        /// </summary>
        public sbyte? TripPurpose { get; set; }
    }
}
