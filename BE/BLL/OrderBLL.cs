using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Order;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public BaseResponseModel Get(int orderState)
        {
            if (orderState < 0 || orderState > 4) {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = "Lỗi tham số truyền vào [0,1,2,3,4]"
                };
            }

            List<OrderResponseModelv2> repon = new List<OrderResponseModelv2>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SP_GetOrderDetailsByState", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@OrderState", orderState));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                repon.Add(new OrderResponseModelv2()
                                {
                                    productname = reader.GetString(reader.GetOrdinal("product_name")),
                                    image = reader.GetString(reader.GetOrdinal("image")),
                                    price = reader.GetDecimal(reader.GetOrdinal("price")),
                                    quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    totalprice = reader.GetDecimal(reader.GetOrdinal("TỔNG TIỀN")),
                                    state = reader.GetInt32(reader.GetOrdinal("state")),
                                });
                            }
                        }
                    }    
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Lấy Thành Công!",
                    Data = repon
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình Lấy Đơn Đặt Hàng: {ex}"
                };
            }
        }
    }
}
