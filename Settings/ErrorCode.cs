using System.ComponentModel;

namespace TravelMasterApi.Settings
{
    public enum ErrorCode
    {
        [Description("Thất bại")]
        FAILED = -1,
        [Description("Thành công")]
        SUCCESS = 0,
        [Description("Thư mục ảnh chưa được cấu hình.")]
        FOLDER_IMAGE_NOT_FOUND,
        [Description("Loại không hợp lệ")]
        TYPE_IS_INVALID,
        [Description("Slug không được để trống")]
        SLUG_IS_EMPTY,
        [Description("Tên không được để trống")]
        NAME_IS_EMPTY,
        [Description("Tên của bạn không được để trống")]
        YOURNAME_IS_EMPTY,
        [Description("Số điện thoại không được để trống")]
        PHONENUMBER_IS_EMPTY,
        [Description("Thời gian không được để trống")]
        TIME_IS_EMPTY,
        [Description("Điểm khởi hành không được để trống")]
        DEPARTURE_IS_EMPTY,
        [Description("Vùng miền không được để trống")]
        REGION_IS_EMPTY,
        [Description("Ngân sách không được để trống")]
        BUDGET_IS_EMPTY,
        [Description("Email không hợp lệ")]
        EMAIL_FORMAT,
        [Description("Số điện thoại không hợp lệ")]
        PHONE_FORMAT,
        [Description("Thời gian nhận phòng không hợp lệ")]
        TIME_CHECKIN_INVALID,
        [Description("Thời gian trả phòng không hợp lệ")]
        TIME_CHECKOUT_INVALID,
        [Description("UUID không được để trống")]
        UUID_IS_EMPTY,
        [Description("Địa điểm không được để trống")]
        LOCATIONS_IS_EMPTY,
        [Description("Thời gian tour không được để trống")]
        TOUR_TIME_IS_EMPTY,
        [Description("Giới thiệu không được để trống")]
        INTRODUCE_IS_EMPTY,
        [Description("Ảnh thu nhỏ không được để trống")]
        THUMBNAIL_IS_EMPTY,
        [Description("Danh mục không hợp lệ")]
        CATEGORIES_INVALID,
        [Description("Địa điểm không hợp lệ")]
        LOCATIONS_INVALID,
        [Description("Vui lòng nhập giá trị tối thiểu, tối đa")]
        VALUE_IS_EMPTY,
        [Description("Lịch trình du thuyền không được để trống")]
        CRUISES_INVALID,
        [Description("Tên đăng nhập hoặc mật khẩu không chính xác.")]
        ACCOUNT_IS_INVALID,
        [Description("Điểm đến không được để trống")]
        DESTINATION_IS_EMPTY,
        [Description("Mật khẩu không chính xác")]
        PASSWORD_IS_INCORRECT,
        [Description("Email không hợp lệ.")]
        EMAIL_IS_INVALID,
        [Description("Tài khoản của bạn đã bị khóa.")]
        ACCOUNT_HAS_LOCKED,
        [Description("Tiêu đề không được để trống")]
        TITLE_IS_EMPTY,
        [Description("Nội dung không được để trống")]
        CONTENT_IS_EMPTY,
        [Description("Id không được để trống")]
        ID_IS_EMPTY,
        [Description("Số ngày không hợp lệ.")]
        DURATION_DAYS_IS_INVALID,
        [Description("Số đêm không hợp lệ.")]
        DURATION_NIGHT_IS_INVALID,
        [Description("Chủ đề đã tồn tại")]
        TOPIC_ALREADY_EXISTS,
        [Description("Bộ lọc đã tồn tại")]
        FILTER_ALREADY_EXISTS,
        [Description("Địa điểm đã tồn tại")]
        LOCATION_ALREADY_EXISTS,
        [Description("Link không được trống")]
        LINK_IS_EMPTY,
        [Description("Vị trí không được trống")]
        POSITION_IS_EMPTY,
        [Description("Mỗi vị trí của danh mục chỉ có 1 baner")]
        BANER_POSITION_ALREADY_EXISTS,
        [Description("Slug của chủ đề không được để trống")]
        SLUG_TOPIC_IS_EMPTY,
        [Description("Slug của khách sạn không được để trống")]
        SLUG_HOTEL_IS_EMPTY,
        [Description("Giá không hợp lệ")]
        PRICE_IS_INVALID,
        [Description("Trạng thái không hợp lệ")]
        STATUS_IS_INVALID,
        [Description("Độ dài tiêu đề không hợp lệ (10-255 kí tự)")]
        NAME_LENGTH_IS_INVALID,
        [Description("Thời lượng không hợp lệ")]
        DURATIONS_IS_INVALÌD,
        [Description("Email đã tồn tại trong hệ thống")]
        EMAIL_ALREADY_EXISTS,
        [Description("Số điện thoại đã tồn tại trong hệ thống")]
        PHONENUMBER_ALREADY_EXISTS,
        [Description("Name cannot exceed 35 characters.")]
        NAME_MAX_LENGTH,

        [Description("Giá trị không hợp lệ")]
        VALUES_IS_INVALID,
        [Description("Không đủ ghế trống")]
        SEAT_ALREADY_EXISTS,
        [Description("Thông tin đăng nhập không chính xác")]
        ACCOUNT_IS_NOT_CORRECT,
       

        //--------------------------------

        [Description("Yêu cầu không hợp lệ")]
        BAD_REQUEST = 400,
        [Description("Không có quyền truy cập")]
        UNAUTHORIZED = 401,
        [Description("Bị cấm")]
        FORBIDDEN = 403,
        [Description("Không tìm thấy đối tượng")]
        NOT_FOUND = 404,
        [Description("File quá lớn")]
        FILE_TOO_LARGE = 413,
        [Description("Lỗi hệ thống")]
        SYSTEM_ERROR = 999,
    }
}
