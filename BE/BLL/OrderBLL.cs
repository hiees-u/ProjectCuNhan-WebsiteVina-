using BLL.Interface;
using BLL.LoginBLL;
using DTO.Order;
using DTO.Responses;
using System.Data.SqlClient;

namespace BLL
{
    public class OrderBLL : IOrder
    {
        public BaseResponseModel Post(OrderRequestModule request)
        {
            try
            {
                if(!request.IsValid())
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Nhập đủ thông tin!"
                    };
                }

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertOrder", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Phone", request.phone);
                        command.Parameters.AddWithValue("@Address_ID", request.adressId);
                        command.Parameters.AddWithValue("@Name_Recipient", request.namerecipient);
                        
                        conn.Open();

                        command.ExecuteNonQuery();
                    }
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Thêm đơn hàng mới thành công!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình Thêm Đơn Đặt Hàng: {ex}"
                };
            }
        }
    }
}
