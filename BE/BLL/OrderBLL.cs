using BLL.Interface;
using BLL.LoginBLL;
using DTO.Order;
using DTO.Responses;
using DTO.Payment;
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

                        command.Parameters.Add(new SqlParameter("@Phone", System.Data.SqlDbType.NVarChar, 11) { Value = request.phone });
                        command.Parameters.Add(new SqlParameter("@Address_ID", System.Data.SqlDbType.Int) { Value = request.addressId });
                        command.Parameters.Add(new SqlParameter("@Name_Recipient", System.Data.SqlDbType.NVarChar, 50) { Value = request.namerecipient });
                        command.Parameters.Add(new SqlParameter("@PaymentStatus", SqlDbType.Bit) { Value = request.paymendStatus});

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
            if (orderState < -1 || orderState > 4) {
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
                                try
                                {
                                    int columnIndex = reader.GetOrdinal("Trạng Thái Thanh Toán");
                                    bool paymentStatus = !reader.IsDBNull(columnIndex) && reader.GetBoolean(columnIndex);
                                }
                                catch (IndexOutOfRangeException ex)
                                {
                                    Console.WriteLine("Cột 'Trạng Thái Thanh Toán' không tồn tại: " + ex.Message);
                                }

                                repon.Add(new OrderResponseModelv2()
                                {
                                    productname = reader.IsDBNull(reader.GetOrdinal("product_name")) ? null : reader.GetString(reader.GetOrdinal("product_name")),
                                    image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")),
                                    price = reader.IsDBNull(reader.GetOrdinal("price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("price")),
                                    quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    totalprice = reader.IsDBNull(reader.GetOrdinal("TỔNG TIỀN")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TỔNG TIỀN")),
                                    state = reader.IsDBNull(reader.GetOrdinal("Trạng Thái")) ? 0 : reader.GetInt32(reader.GetOrdinal("Trạng Thái")),
                                    orderid = reader.IsDBNull(reader.GetOrdinal("Mã Đơn Hàng")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mã Đơn Hàng")),
                                    pricehistoryid = reader.IsDBNull(reader.GetOrdinal("Mã Giá")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mã Giá")),
                                    paymentStatus = !reader.IsDBNull(reader.GetOrdinal("Trạng Thái Thanh Toán")) && reader.GetBoolean(reader.GetOrdinal("Trạng Thái Thanh Toán"))
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

        public BaseResponseModel Delete(int OrderId, int PriceHistory)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SP_DeleteOrderDetailState", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                        command.Parameters.Add(new SqlParameter("@PriceHistoryId", PriceHistory));

                        command.ExecuteNonQuery();
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Xóa Đơn Hàng Thành Công!"
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

        public async Task<bool> UpdateOrderPaymentStatus(string orderId, MomoInfoModel paymentInfo)
        {
            using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
            {
                using (var command = new SqlCommand("SP_UpdatePaymentStatus", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@OrderId", orderId));
                    command.Parameters.Add(new SqlParameter("@Amount", paymentInfo.Amount));
                    command.Parameters.Add(new SqlParameter("@DatePaid", paymentInfo.DatePaid));
                    command.Parameters.Add(new SqlParameter("@PaymentStatus", true)); // Chuyển trạng thái thành công

                    conn.Open();
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

    }
}
