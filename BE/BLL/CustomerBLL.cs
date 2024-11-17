using BLL.Interface;
using BLL.LoginBLL;
using DTO.Customer;
using DTO.Responses;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    public class CustomerBLL : ICustomer
    {
        public BaseResponseModel Get(
            int? TypeCustomerId,
            int pageNumber = 1,
            int pageSize = 8
        ) {
            try
            {
                List<CustomerResponseModule> customers = new List<CustomerResponseModule>();
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var command = new SqlCommand("SP_GetCustomer", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@TypeCustomerID", SqlDbType.Int)
                        {
                            Value = TypeCustomerId.HasValue ? (object)TypeCustomerId.Value : DBNull.Value,
                        });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new CustomerResponseModule()
                                {
                                    AccountName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                    FullName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Gender = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                    TypeCustomerId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                    TypeCustomerName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    AddressId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                                    AddressString = reader.IsDBNull(8) ? null : reader.GetString(8)
                                });
                            }
                        }
                    }
                }

                int totalPages = (int)Math.Ceiling((double)customers.Count / pageSize);

                //Phân trang
                customers = customers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = customers.Count() > 0 ? "Lấy ra danh sách Khách Hàng Thành Công!" : "Không tìm thấy khách hàng nào",
                    Data = new
                    {
                        customers,
                        totalPages,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex.Message}",
                    Data = null
                };
            }
        }

        public BaseResponseModel Put(CustomerRequestModule req)
        {
            if(req.isValid())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_UpdateCustomer", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@AccountName", SqlDbType.NVarChar, 50) { Value = req.AccountName });
                            cmd.Parameters.Add(new SqlParameter("@NewTypeCustomerID", SqlDbType.Int) { Value = req.TypeCustomerId });

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = $"Cập nhật thông tin Khách Hàng {req.AccountName} thành công"
                    };
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
                Message = "Vui lòng kiểm tra lại thông tin..!"
            };
        }

        public BaseResponseModel Delete(string userName)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SP_DeleteCustomer", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@AccountName", SqlDbType.NVarChar, 50) { Value = userName });

                        cmd.ExecuteNonQuery();
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = $"Xóa Khách Hàng {userName} thành công"
                };
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
