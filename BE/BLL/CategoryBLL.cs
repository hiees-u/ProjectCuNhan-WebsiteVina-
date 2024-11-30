using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Category;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class CategoryBLL : ICategory
    {
        public BaseResponseModel GetCateNameByProductID(int productID)
        {
            try
            {
                string categoryName = string.Empty;
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT dbo.GetCategoryName(@ProductID)", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int) { Value = productID });

                        object result = cmd.ExecuteScalar(); 
                        if (result != DBNull.Value && result != null) { 
                            categoryName = (string)result; 
                        }
                    }
                }
                if(!string.IsNullOrWhiteSpace(categoryName))
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Thành Công",
                        Data = categoryName,
                    };
                }

                return new BaseResponseModel ()
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

        public BaseResponseModel GetTop10()
        {
            try
            {
                var categories = new List<Category>();
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT * FROM dbo.GetTop10Categories()", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cate = new Category
                                {
                                    CategoryId = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1),
                                    Image = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    DeleteTime = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                    ModifiedBy = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    CreateTime = reader.GetDateTime(5),
                                    ModifiedTime = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                };
                                categories.Add(cate);
                            }
                        }
                    }
                    if (categories.Count() == 0)
                    {
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
                        Data = categories
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
        
        public BaseResponseModel GetPagition(int? cateId = null, string? cateName = null, int pageNumber = 1, int pageSize = 8)
        {
            try
            {
                List<CategoryReponseModule> lst = new List<CategoryReponseModule>();

                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SP_SelectCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        if(cateId.HasValue)
                        {
                            command.Parameters.Add(new SqlParameter("@category_id", SqlDbType.Int) { Value = cateId });
                        }

                        if (!string.IsNullOrEmpty(cateName))
                        {
                            command.Parameters.Add(new SqlParameter("@category_name", SqlDbType.NVarChar, 30) { Value = cateName });
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) {
                                CategoryReponseModule cate = new CategoryReponseModule()
                                {
                                    CategoryId = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1)
                                };
                                lst.Add(cate);
                            }
                        }
                    }
                }

                int totalPages = 0;

                if (pageSize != 0)
                {
                    //Phân trang
                    totalPages = (int)Math.Ceiling((double)lst.Count / pageSize);
                    lst = lst.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Lấy Danh Sách LOẠI SẢN PHẨM Thành Công!",
                    Data = new
                    {
                        data = lst,
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
    
        public BaseResponseModel Put(CategoryRequestModule req)
        {
            if(req.validateCateRes())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_UpdateCategory", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@category_id", SqlDbType.Int)
                            {
                                Value = req.CategoryId
                            });

                            cmd.Parameters.Add(new SqlParameter("@category_name", SqlDbType.NVarChar)
                            {
                                Value = req.CategoryName
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

        public BaseResponseModel Delete(int cateId)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("SP_DeleteCategory", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@category_id", SqlDbType.Int) { Value = cateId });

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
                    Message = $"Không thể xóa. Còn sản phẩm"
                };
            }
        }

        public BaseResponseModel Post(string CategoryName)
        {
            if (!string.IsNullOrEmpty(CategoryName))
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_InsertCategory", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@category_name", SqlDbType.NVarChar, 30) {
                                Value = CategoryName 
                            });

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                                return new BaseResponseModel()
                                {
                                    IsSuccess = true,
                                    Message = "Thêm loại sản phẩm Thành Công"
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
                Message = "Vui lòng kiểm tra Tên Loại sản phẩm!"
            };
        }
    }
}
