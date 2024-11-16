using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Responses;
using DTO.Supplier;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class SupplierBLL : ISupplier
    {
        public BaseResponseModel GetById(int supplierID)
        {
            Supplier supplier = new Supplier();
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.GetSupplierById(@SupplierID)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                supplier = new Supplier
                                {
                                    SupplierId = (int)reader["SupplierID"],
                                    SupplierName = reader["SupplierName"].ToString(),
                                    AddressId = (int)reader["AddressId"],
                                    DeleteTime = reader["DeleteTime"] != DBNull.Value ? (DateTime?)reader["DeleteTime"] : null,
                                    DeleteBy = reader["DeleteBy"] != DBNull.Value ? reader["DeleteBy"].ToString() : null,
                                };
                            } 
                        }
                    }
                }
                if (supplier != null)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Thành Công",
                        Data = supplier,
                    };
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Không tìm thấy!",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy tất cả sản phẩm: {ex}"
                };
            }
        }
   
        public BaseResponseModel GetPagition(int? productID)
        {
            List<SupplierResponseModule> lst = new List<SupplierResponseModule>();
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetSupplier", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (productID.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@ProductID", productID);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SupplierResponseModule result = new SupplierResponseModule()
                                {
                                    SupplierId = (int)reader["SupplierID"],
                                    SupplierName = reader["SupplierName"].ToString(),
                                    AddressId = (int)reader["AddressID"]
                                };
                                lst.Add(result);
                            }
                        }
                    }
                }
                if (lst.Count > 0)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Lấy Danh Sách Nhãn Hàng Thành Công!!",
                        Data = lst
                    };
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Không tìm tháy nhãn hàng nào...",
                    Data = lst
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

        public BaseResponseModel Post(SupplierRequestModule req)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_InsertSupplier", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@SupplierName", SqlDbType.NVarChar, 30) { Value = req.SupplierName });
                        cmd.Parameters.Add(new SqlParameter("@AddressID", SqlDbType.Int) { Value = req.AddressId });

                        int rowAffected = cmd.ExecuteNonQuery();

                        if (rowAffected > 0)
                        {
                            return new BaseResponseModel()
                            {
                                IsSuccess = true,
                                Message = "Thêm Nhà Cung cấp thành công!"
                            };
                        }
                        return new BaseResponseModel()
                        {
                            IsSuccess = false,
                            Message = "Đã có lỗi xảy ra..."
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

        public BaseResponseModel Put(SupplierResponseModule req)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateSupplier", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@SupplierID", SqlDbType.Int)
                        {
                            Value = req.SupplierId
                        });

                        cmd.Parameters.Add(new SqlParameter("@SupplierName", SqlDbType.NVarChar, 30)
                        {
                            Value = req.SupplierName
                        });

                        cmd.Parameters.Add(new SqlParameter("@AddressID", SqlDbType.Int)
                        {
                            Value = req.AddressId
                        });

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if(rowsAffected > 0)
                        {
                            return new BaseResponseModel
                            {
                                IsSuccess = true,
                                Message = "Cập Nhật Thành Công!!"
                            };
                        }
                        return new BaseResponseModel()
                        {
                            IsSuccess = false,
                            Message = "Vui lòng kiểm tra lại!"
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

        public BaseResponseModel Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteSupplier", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@SupplierID", SqlDbType.Int) { Value = id });

                        cmd.ExecuteNonQuery();

                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Xóa Thành Công!!"
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
