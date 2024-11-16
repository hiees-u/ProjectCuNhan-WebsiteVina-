using BLL.Interface;
using BLL.LoginBLL;
using DTO.CustomerType;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class CustomerTypeBLL : ICustomerType
    {
        public BaseResponseModel Get(int? customerID)
        {
            try
            {
                List<CustomerTypeResponseModule> result = new List<CustomerTypeResponseModule>();
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetCustomerType", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (customerID.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", customerID);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new CustomerTypeResponseModule()
                                {
                                    TypeCustomerId = reader.GetInt32(0),
                                    TypeCustomerName = reader.GetString(1)
                                });
                            }
                        }
                    }
                    return result.Count > 0 ? new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Lấy Ra Loại Khách Hàng Thành Công!",
                        Data = result
                    } : new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy Loại Khách Hàng nào..!",
                        Data = result
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex}"
                };
            }
        }
    
        public BaseResponseModel Post(CustomerTypeRequestModule req)
        {
            if(req.isValidate())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand("SP_InsertCustomerType", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CustomerTypeName", req.TypeCustomerName);

                            try
                            {
                                int rowAffected = cmd.ExecuteNonQuery();

                                return rowAffected > 0 ?
                                    new BaseResponseModel { IsSuccess = true, Message = "Thêm Loại Khách Hàng Thành Công!!" } :
                                    new BaseResponseModel { IsSuccess = false, Message = "Thêm Loại Khách Hàng Thất Bại!!" };
                            }
                            catch (Exception ex)
                            {
                                return new BaseResponseModel()
                                {
                                    IsSuccess = false,
                                    Message = $"Lỗi trong quá trình: {ex}"
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = $"Lỗi trong quá trình: {ex}"
                    };
                }
            }
            return new BaseResponseModel()
            {
                IsSuccess = false,
                Message = "Vui lòng kiểm tra lại tên Loại Khách Hàng..!"
            };
        }

        public BaseResponseModel Put(CustomerTypeResponseModule req)
        {
            if (req.isValidate())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand("SP_UpdateCustomerType", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int) { Value = req.TypeCustomerId });
                            cmd.Parameters.Add(new SqlParameter("@CustomerTypeName", SqlDbType.NVarChar, 30) { Value = req.TypeCustomerName });

                            int rowAffected = cmd.ExecuteNonQuery();

                            return rowAffected > 0 ? new BaseResponseModel()
                            {
                                IsSuccess = true,
                                Message = "Cập nhật Loại Khách Hàng Thành Công..!"
                            } : new BaseResponseModel()
                            {
                                IsSuccess = false,
                                Message = "Vui lòng kiểm tra lại"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = $"Lỗi trong quá trình: {ex}"
                    };
                }
            }
            return new BaseResponseModel()
            {
                IsSuccess = false,
                Message = "Kiểm tra lại thông tin...!"
            };
        }

        public BaseResponseModel Delete(int? TypeCustomerId)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteCustomerType", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CustomerTypeID", SqlDbType.Int) { Value = TypeCustomerId });

                        SqlParameter resultParameter = new SqlParameter(); 
                        resultParameter.ParameterName = "@Result"; 
                        resultParameter.SqlDbType = SqlDbType.Int; 
                        resultParameter.Direction = ParameterDirection.Output; 
                        cmd.Parameters.Add(resultParameter);

                        cmd.ExecuteNonQuery();

                        int result = (int)cmd.Parameters["@Result"].Value;

                        return result == 1 ? new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Xóa Loại Khách Hàng Thành Công..!"
                        } : new BaseResponseModel()
                        {
                            IsSuccess = false,
                            Message = "Không thể xóa loại khách hàng này vì vẫn còn khách hàng liên quan"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex}"
                };
            }
        }
    }
}
