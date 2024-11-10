using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class SubCategoryBLL : ISubCategory
    {
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
    }
}
