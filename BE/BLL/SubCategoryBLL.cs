using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Responses;
using DTO.Subcategory;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class SubCategoryBLL : ISubCategory
    {
        public BaseResponseModel GetPagition(int? subCateId = null, string? subCateName = null, int pageNumber = 1, int pageSize = 8)
        {
            try
            {
                List<SubcategoryReponseModel> subCates = new List<SubcategoryReponseModel>();

                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetSubCategory", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if(subCateId.HasValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@subCategory_id", SqlDbType.Int) { Value = subCateId });
                        }
                        if(!string.IsNullOrEmpty(subCateName))
                        {
                            cmd.Parameters.Add(new SqlParameter("@subCategory_name", SqlDbType.NVarChar) { Value = subCateName });
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SubcategoryReponseModel res = new SubcategoryReponseModel()
                                {
                                    SubCategoryId = reader.GetInt32(0),
                                    SubCategoryName = reader.GetString(1),
                                };
                                subCates.Add(res); 
                            }
                        }
                    }
                }
                //Phân trang
                int totalPages = (int)Math.Ceiling((double)subCates.Count / pageSize);
                subCates = subCates.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Lấy Danh Sách LOẠI SẢN PHẨM PHỤ Thành Công!",
                    Data = new
                    {
                        data = subCates,
                        totalPages
                    }
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

        public BaseResponseModel GetSubCateNameByProductID(int productID)
        {
            try
            {
                string subCategoryName = string.Empty;
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT dbo.GetSubCategoryName(@ProductID)", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int) { Value = productID });

                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            subCategoryName = (string)result;
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(subCategoryName))
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Thành Công",
                        Data = subCategoryName,
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
                    Message = $"Lỗi trong quá trình: {ex}"
                };
            }
        }

        public BaseResponseModel GetTop10()
        {
            try
            {
                var subCategories = new List<SubCategory>();
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT * FROM dbo.GetTop10SubCategories()", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var subCategory = new SubCategory
                                {
                                    SubCategoryId = reader.GetInt32(0),
                                    SubCategoryName = reader.GetString(1),
                                    Image = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    DeleteTime = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                    ModifiedBy = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    CreateTime = reader.GetDateTime(5),
                                    ModifiedTime = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                };
                                subCategories.Add(subCategory);
                            }
                        }
                    }
                    if (subCategories.Count() == 0) {
                        return new BaseResponseModel()
                        {
                            IsSuccess = false,
                            Message = "Không tìm thấy!"
                        };
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Lấy thành công!",
                        Data = subCategories
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
    
        public BaseResponseModel Put(SubcategoryRequestModel req)
        {
            if (req.isValidate())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_UpdateSubCategory", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@subCategory_id", SqlDbType.Int)
                            {
                                Value = req.SubCategoryId
                            });

                            cmd.Parameters.Add(new SqlParameter("@subCategory_name", SqlDbType.NVarChar)
                            {
                                Value = req.SubCategoryName
                            });

                            //thực thi
                            cmd.ExecuteNonQuery();

                            return new BaseResponseModel()
                            {
                                IsSuccess = true,
                                Message = "Cập Nhật Thành Công!"
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
                Message = "Lỗi Validate.."
            };
        }

        public BaseResponseModel Delete(int subCateId)
        {
            //SP_DeleteSubCategory
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SP_DeleteSubCategory", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@subCategory_id", SqlDbType.Int)
                        {
                            Value = subCateId
                        });

                        //thực thi
                        //cmd.ExecuteNonQuery();
                        SqlParameter resultParameter = new SqlParameter();
                        resultParameter.ParameterName = "@ReturnVal";
                        resultParameter.SqlDbType = SqlDbType.Int;
                        resultParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(resultParameter);

                        cmd.ExecuteNonQuery();

                        int result = (int)cmd.Parameters["@ReturnVal"].Value;

                        if (result == 0)
                        {
                            return new BaseResponseModel()
                            {
                                IsSuccess = false,
                                Message = "Không thể xóa. Còn sản phẩm liên quan!!"
                            };
                        }
                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Xóa thành công!"
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

        public BaseResponseModel Post(string SubcategoryName)
        {
            if (!string.IsNullOrEmpty(SubcategoryName))
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_InsertSubCategory", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@subCategory_name", SqlDbType.NVarChar, 30)
                            {
                                Value = SubcategoryName
                            });

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                                return new BaseResponseModel()
                                {
                                    IsSuccess = true,
                                    Message = "Thêm loại sản phẩm phụ Thành Công"
                                };
                        }
                    }
                    return new BaseResponseModel
                    {
                        IsSuccess = false,
                        Message = "Thêm loại sản phẩm thất bại..."
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
                Message = "Vui lòng kiểm tra Tên Loại sản phẩm phụ!"
            };
        }
    }
}
