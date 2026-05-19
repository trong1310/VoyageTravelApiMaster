namespace TravelMasterApi.Models.Admin.Response
{
    public class GetContactResponseMessage
    {
        public string? Uuid { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Title { get; set; }
        public sbyte? Type { get; set; }
        public sbyte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
    public class DetailContactResponseMessage
    {
        public string? Uuid { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? NobleName { get; set; }
        public string? Title { get; set; }
        public sbyte? Type { get; set; }
        public sbyte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        

    }
}
