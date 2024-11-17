using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Employee;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class EmployeeBLL : IEmployee
    {
        public BaseResponseModel Get(int? departmentID = null, int? employeeTypeID = null)
        {
            try
            {
                List<EmployeeResponseModule> employees = new List<EmployeeResponseModule>();
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var command = new SqlCommand("SP_GetEmployees", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int)
                        {
                            Value = departmentID.HasValue ? (object)departmentID.Value : DBNull.Value,
                        });

                        command.Parameters.Add(new SqlParameter("@EmployeeTypeID", SqlDbType.Int)
                        {
                            Value = employeeTypeID.HasValue ? (object)employeeTypeID.Value : DBNull.Value,
                        });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(new EmployeeResponseModule()
                                {
                                    employId = reader.GetInt32(0),
                                    accountName = reader.GetString(1),
                                    fullName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    employeeTypeId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                                    employeeTypeName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    departmentId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                                    departmentName = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    gender = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                                    addressId = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                                    addressName = reader.IsDBNull(9) ? null : reader.GetString(9),
                                });
                            }
                        }
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = employees.Count > 0 ? "Lấy danh sách nhân viên thành công..!" : "Không tìm thấy nhân viên..!",
                    Data = employees
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

        public BaseResponseModel Post(EmployeeRequestPostModule req)
        {
            if(req.isValid())
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_InsertEmployee", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@EmployeeTypeID", req.employeeTypeId));
                        cmd.Parameters.Add(new SqlParameter("@DepartmentID", req.departmentId));
                        cmd.Parameters.Add(new SqlParameter("@AccountName", req.accountName));

                        cmd.ExecuteNonQuery();
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Thêm Nhân Viên Thành Công..!"
                };
            }
            return new BaseResponseModel()
            {
                IsSuccess = false,
                Message = "Kiểm tra lại thông tin..!"
            };
        }

        public BaseResponseModel Delete(string accountName)
        {
            if(!string.IsNullOrEmpty(accountName))
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("SP_DeleteEmployee", conn))
                        {
                            cmd.CommandType= CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@AccountName", accountName));
                            cmd.ExecuteNonQuery ();
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Xóa Thành Công..!"
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
            return new BaseResponseModel()
            {
                IsSuccess = false,
                Message = "Vui lòng kiểm tra lại thông tin..!"
            };
        }

        public BaseResponseModel Put(EmployeeRequestPutModule req)
        {
            if (req.isValid())
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("SP_UpdateEmployee", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@AccountName", req.accountName));
                            cmd.Parameters.Add(new SqlParameter("@EmployeeTypeID", req.employeeTypeId));
                            cmd.Parameters.Add(new SqlParameter("@DepartmentID", req.departmentId));
                            cmd.Parameters.Add(new SqlParameter("@FullName", req.fullName));
                            cmd.Parameters.Add(new SqlParameter("@Email", req.email));
                            cmd.Parameters.Add(new SqlParameter("@AddressID", req.addressId));
                            cmd.Parameters.Add(new SqlParameter("@Phone", req.phone));
                            cmd.Parameters.Add(new SqlParameter("@Gender", req.gender));

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "cập nhật thành công..!"
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
            return new BaseResponseModel()
            {
                IsSuccess = false,
                Message = "Kiểm tra lại thông tin..!"
            };
        }
    }
}
