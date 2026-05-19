using TravelMasterApi.Base.Utils;
using TravelMasterApi.Settings;

namespace TravelMasterApi.Base.BaseMessages
{
    public class BaseResponseMessage
    {
        public Error error { get; set; } = new Error();
        public class Error
        {
            /// <summary>
            /// Mã code lỗi
            /// </summary>
            public ErrorCode Code { get; set; }
            /// <summary>
            /// Mô tả lỗi
            /// </summary>
            public string Message { get; set; }
            public Error(ErrorCode code = ErrorCode.SUCCESS)
            {
                this.Code = code;
                this.Message = code.ToDescriptionString();
            }
        }       

    }
    public class BaseResponseMessage<T> : BaseResponseMessage
    {
        /// <summary>
        /// Dữ liệu đầu ra
        /// </summary>
        public T? Data { get; set; }
    }
    public class BaseResponseMessageItem<T>
    {
        public List<T> Items { get; set; } = new List<T>();
    }
    /// <summary>
    /// mess phân trang
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponseMessagePage<T>
    {
        /// <summary>
        /// Danh sách Items
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();
        /// <summary>
        /// Phân trang
        /// </summary>
        public Paginations Pagination { get; set; } = new Paginations();
        public class Paginations
        {
            /// <summary>
            /// Tổng số item trên 1 trang
            /// </summary>
            public int TotalCount { get; set; } = 0;
            /// <summary>
            /// Tổng số trang
            /// </summary>
            public int TotalPage { get; set; } = 0;
        }
    }
	public class BaseUpdateCode<T>
	{
		public int Code { get; set; }
		public QueryBase query { get; set; } = null;
		public List<T> Results { get; set; } = new List<T>();
		public string err { get; set; }
	}
	public class QueryBase
	{
		public int max_time { get; set; } = 0;
	}
	public class BaseUpdateCodeData<T>
	{
		public int Code { get; set; } = 0;
		public T? Results { get; set; }
	}
	public class DataResponse<T> : BaseResponse
	{
		public T Data { get; set; }
	}
	public class BaseResponse
	{
		public ErrorCode Code { get; set; } = ErrorCode.SUCCESS;
		public string Message { get; set; } = ErrorCode.SUCCESS.ToDescriptionString();
	}
}
