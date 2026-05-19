namespace TravelMasterApi.Base.Pages
{
    /// <summary>
    /// Thông tin hiển thị phân trang
    /// </summary>
    public interface PagingRequest
    {
        /// <summary>
        /// Số phần tử trên một trang, đặt -1 nếu muốn lấy toàn bộ danh sách
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Trang cần lấy
        /// </summary>
        public int? Page { get; set; }
    }

    /// <summary>
    /// Dữ liệu thông tin trả về khi phân trang
    /// </summary>
    public interface PagingResponse
    {
        /// <summary>
        /// Tổng số phần tử có trong
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPage { get; set; }
    }
}
