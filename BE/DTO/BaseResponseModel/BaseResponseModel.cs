namespace DTO.Responses
{
    public class BaseResponseModel
    {
        public bool IsSuccess { get; set; } // Để xác định thao tác có thành công hay không
        public string? Message { get; set; } // Thông báo phản hồi
        public object? Data { get; set; } = null; // Dữ liệu trả về, có thể là bất kỳ loại nào
    }
}
