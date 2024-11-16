using BLL.Interface;
using BLL.LoginBLL;
using DTO.Employee;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class EmployeeBLL : IEmployee
    {
        public BaseResponseModel Get(int? EmployeeTypeID, int? DepartmentID, int pageNumber = 1, int pageSize = 8)
        {
            try
            {
                List<EmployeeResponseModule> list = new List<EmployeeResponseModule>();
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetEmployees", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int)
                        {
                            Value = DepartmentID.HasValue ? DepartmentID.Value : DBNull.Value,
                        });
                        cmd.Parameters.Add(new SqlParameter("@EmployeeTypeID", SqlDbType.Int)
                        {
                            Value = EmployeeTypeID.HasValue ? EmployeeTypeID.Value : DBNull.Value,
                        });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new EmployeeResponseModule()
                                {
                                    EmployeeId = reader.GetInt32(0),
                                    AccountName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    FullName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    EmployeeTypeId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    EmployeeTypeName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    DepartmentId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                    DepartmentName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    Gender = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                                    AddressId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    AddressName = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                });
                            }
                        }
                    }

                    int totalPages = (int)Math.Ceiling((double)list.Count / pageSize);
                    list = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return list.Count > 0 ? 
                        new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Lấy Thành Công Danh Sách Nhân Viên..!",
                            Data = new
                            {
                                list,
                                totalPages,
                            }
                        } : new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Không Tồn Tại Nhân Viên Nào..!"
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

        public BaseResponseModel Post(EmployeeRequestModule req)
        {
            if (req.isValidate())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_InsertEmployee", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@EmployeeTypeID", SqlDbType.Int) { Value = req.EmployeeTypeId });
                            cmd.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = req.DepartmentId });
                            cmd.Parameters.Add(new SqlParameter("@AccountName", SqlDbType.VarChar, 25) { Value = req.AccountName });

                            // Thêm biến đầu ra
                            var rowCountParam = new SqlParameter("@RowCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                            cmd.Parameters.Add(rowCountParam);

                            // Thực thi stored procedure
                            cmd.ExecuteNonQuery();
                            int rowAffected = (int)rowCountParam.Value;

                            return new BaseResponseModel()
                            {
                                IsSuccess = rowAffected > 0,
                                Message = rowAffected > 0 ? "Thêm Nhân Viên Mới Thành Công!" : "Vui lòng kiểm tra lại..!"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = $"Lỗi trong quá trình: {ex.Message}"
                    };
                }
            }
            return new BaseResponseModel()
            {
                IsSuccess = false,
                Message = "Kiểm tra lại thông tin..!"
            };
        }

        public BaseResponseModel Put(EmployeeRequestPutModule req)
        {
            if (req.IsValid())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_UpdateEmployee", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@AccountName", SqlDbType.NVarChar, 50) { Value = req.AccountName });
                            cmd.Parameters.Add(new SqlParameter("@EmployeeTypeID", SqlDbType.Int) { Value = req.EmployeeTypeId });
                            cmd.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = req.DepartmentId });
                            cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = req.FullName });
                            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = req.Email });
                            cmd.Parameters.Add(new SqlParameter("@AddressID", SqlDbType.Int) { Value = req.AddressId });
                            cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 15) { Value = req.Phone });
                            cmd.Parameters.Add(new SqlParameter("@Gender", SqlDbType.Int) { Value = req.Gender });

                            // Thêm biến đầu ra
                            var rowCountParam = new SqlParameter("@RowCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                            cmd.Parameters.Add(rowCountParam);

                            // Thực thi stored procedure
                            cmd.ExecuteNonQuery();
                            int rowAffected = (int)rowCountParam.Value;

                            return new BaseResponseModel()
                            {
                                IsSuccess = rowAffected > 0,
                                Message = rowAffected > 0 ? "Cập nhật nhân viên thành công!" : "Vui lòng kiểm tra lại..!"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = $"Lỗi trong quá trình: {ex.Message}"
                    };
                }
            }
            return new BaseResponseModel()
            {
                IsSuccess = false,
                Message = "Kiểm tra lại thông tin..!"
            };
        }

        public BaseResponseModel Delete(string accountName)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SP_DeleteEmployee", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số cần thiết
                        cmd.Parameters.Add(new SqlParameter("@AccountName", SqlDbType.NVarChar, 50) { Value = accountName });

                        // Thêm biến đầu ra
                        var rowCountParam = new SqlParameter("@RowCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(rowCountParam);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();
                        int rowAffected = (int)rowCountParam.Value;

                        return new BaseResponseModel()
                        {
                            IsSuccess = rowAffected > 0,
                            Message = rowAffected > 0 ? "Xóa nhân viên thành công!" : "Xóa nhân viên thất bại. Vui lòng kiểm tra lại..!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex.Message}"
                };
            }
        }

    }
}
