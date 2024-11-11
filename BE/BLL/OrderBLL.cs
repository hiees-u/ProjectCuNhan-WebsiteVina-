using BLL.Interface;
using BLL.LoginBLL;
using DTO.Order;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class OrderBLL : IOrder
    {
        public BaseResponseModel Post(OrderRequestModule request)
        {
            try
            {
                if (!request.IsValid())
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Nhập đủ thông tin!"
                    };
                }

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertOrderWithDetails", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        //command.Parameters.AddWithValue("@Phone", request.phone);
                        command.Parameters.Add(new SqlParameter("@Phone", System.Data.SqlDbType.NVarChar, 11) { Value = request.phone });
                        command.Parameters.Add(new SqlParameter("@Address_ID", System.Data.SqlDbType.Int) { Value = request.addressId });
                        command.Parameters.Add(new SqlParameter("@Name_Recipient", System.Data.SqlDbType.NVarChar, 50) { Value = request.namerecipient });

                        SqlParameter tvpParam = command.Parameters.AddWithValue("@ProductQuantities", CreateProductQuantityDataTable(request.products));
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dbo.ProductQuantityType";

                        conn.Open();

                        command.ExecuteNonQuery();
                    }
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Đặt hàng thành công!"
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

        private DataTable CreateProductQuantityDataTable(List<ProductQuantity> productQuantities)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PriceHistoryId", typeof(int));
            dt.Columns.Add("Quantity", typeof(int));

            foreach (var pq in productQuantities)
            {
                dt.Rows.Add(pq.PriceHistoryId, pq.Quantity);
            }
            return dt;
        }
    }
}
