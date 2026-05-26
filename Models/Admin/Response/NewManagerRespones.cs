namespace TravelMasterApi.Models.Admin.Response
{
    public class GetNewManagerResponesModels
    {
        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string Slug { get; set; } = null!;

        public ulong IsHot { get; set; }
        public string? ThumbNail { get; set; }

        /// <summary>
        /// 1 tin tức khuyến mãi, 2dịch vụ khác, 3 kinh nghiệm du lịch
        /// 
        /// </summary>
        public sbyte Type { get; set; }

        public string? SeoDescription { get; set; }
    }
    public class DetailNewManagerResponesModels
    {

        public string Title { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public ulong IsHot { get; set; }

        public string Content { get; set; } = null!;
        public string? ThumbNail { get; set; }

        /// <summary>
        /// 1 tin tức khuyến mãi, 2dịch vụ khác, 3 kinh nghiệm du lịch
        /// 
        /// </summary>
        public sbyte Type { get; set; }

        public string? SeoDescription { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class GetNewTypeResponeModels
    {
        public string? Name { get; set; }
        public sbyte? Id { get; set; }
    }
    public class ListEmailSubscribeResponesModels
    {
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
