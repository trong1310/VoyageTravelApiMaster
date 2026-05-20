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

    public class DetailBookingManagerResponeModels
    {
        public string Uuid { get; set; }
        public sbyte? State { get; set; }
        internal string? SlugOwner { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ServiceName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SpecialRequirements { get; set; }
        public string? Email { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TotalCustomer { get; set; } = 0;
    }
}
