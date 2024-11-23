using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Department;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class DepartmentBLL : IDepartment
    {
        public BaseResponseModel Get(int pageNumber, int pageSize)
        {
            try
            {
                List<DepartmentRequestModule> result = new List<DepartmentRequestModule>();
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetDepartment", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DepartmentRequestModule dep = new DepartmentRequestModule()
                                {
                                    DepartmentId = reader.GetInt32(0),
                                    DepartmentName = reader.GetString(1),
                                    sumEmployee = reader.GetInt32(2),
                                };

                                result.Add(dep);
                            }
                        }
                    }
                }

                int totalPages = 0;

                if (pageNumber > 0)
                {
                    totalPages = (int)Math.Ceiling((double)result.Count / pageSize);

                    //Phân trang
                    result = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                return result.Count > 0 ?
                    new BaseResponseModel() {
                        IsSuccess = true, 
                        Message = "Lấy danh sách phòng thành công..!", 
                        Data = new {
                            data = result,
                            totalPages,
                        } 
                    } :
                    new BaseResponseModel() { IsSuccess = true, Message = "Không tìm thấy phòng ban nào..!" };
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

        public BaseResponseModel Put(DepartmentRequestModule req)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateDepartmentName", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = req.DepartmentId });
                        cmd.Parameters.Add(new SqlParameter("@NewDepartmentName", SqlDbType.NVarChar, 30) { Value = req.DepartmentName });

                        cmd.ExecuteNonQuery();
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Cập nhật thành công!"
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

        public BaseResponseModel Post(string departmentName)
        {
            if(!string.IsNullOrEmpty(departmentName))
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand("SP_InsertDepartment", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@DepartmentName", SqlDbType.NVarChar, 50) { Value = departmentName });

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Thêm Phòng Ban Thành Công..!"
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
                Message = "Kiểm tra lại tên phòng ban..!"
            };
        }

        public BaseResponseModel Delete(int? deparId)
        {
            if (deparId.HasValue)
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand("SP_SoftDeleteDepartment", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = deparId });

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Xóa Phòng Ban Thành Công..!"
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
                Message = "Kiểm tra lại mã phòng ban..!"
            };
        }
    }
}
