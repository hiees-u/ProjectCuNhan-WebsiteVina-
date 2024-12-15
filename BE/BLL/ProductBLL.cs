using BLL.Interface;
using BLL.LoginBLL;
using DTO.Product;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class ProductBLL : IProduct
    {
        public BaseResponseModel GetProducts(
            int? productId,
            int? cateId,
            int? subCateId,
            int? supplierId,
            string? productName,
            int pageNumber = 1,
            int pageSize = 8,
            int sortByName = 0 /*1: tăng, -1: giảm, 0:*/, 
            int sortByPrice = 0 /*1: tăng, -1: giảm, 0:*/
        )
        {
            List<ProductViewUserResponseModel> products = new List<ProductViewUserResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var command = new SqlCommand("SP_GetAllProducts", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Thực hiện lệnh
                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read())
                            {
                                ProductViewUserResponseModel product = new ProductViewUserResponseModel
                                {
                                    ProductId = Convert.ToInt32(reader["product_id"]),
                                    ProductName = reader["product_name"] as string ?? string.Empty,
                                    Image = Convert.ToString(reader["image"]),
                                    TotalQuantity = Convert.ToInt32(reader["totalQuantity"]),
                                    CategoryId = Convert.ToInt32(reader["Category_id"]),
                                    Supplier = Convert.ToInt32(reader["Supplier"]),
                                    SubCategoryId = Convert.ToInt32(reader["SubCategoryID"]),
                                    ExpriryDate = Convert.ToDateTime(reader["ExpriryDate"]),
                                    Description = Convert.ToString(reader["Description"]),
                                    ModifiedBy = Convert.ToString(reader["ModifiedBy"]),
                                    CreateTime = Convert.ToDateTime(reader["CreateTime"]),
                                    ModifiedTime = Convert.ToDateTime(reader["ModifiedTime"]),
                                    DeleteTime = reader["DeleteTime"] != DBNull.Value ? Convert.ToDateTime(reader["DeleteTime"]) : null,
                                    Price = Convert.ToDecimal(reader["price"]),
                                    priceHistoryId = Convert.ToInt32(reader["priceHistoryId"]),
                                    categoryName = Convert.ToString(reader["category_name"]) as string ?? string.Empty,
                                    subCategoryName = Convert.ToString(reader["SubCategoryName"]) as string ?? string.Empty
                                };

                                products.Add(product);
                            }
                        }

                        //check if product_id has values
                        if (productId.HasValue)
                        {
                            products = products.Where(p => p.ProductId.Equals(productId)).ToList();
                        }

                        //check if subCateId has value
                        if (subCateId.HasValue)
                        {
                            products = products.Where(p => p.SubCategoryId == subCateId).ToList();
                        }

                        //check if cate_id has value
                        if (cateId.HasValue)
                        {
                            products = products.Where(p => p.CategoryId == cateId).ToList();
                        }

                        if (supplierId.HasValue)
                        {
                            products = products.Where(p => p.Supplier == supplierId).ToList();
                        }

                        //check if productName has value
                        if (!string.IsNullOrEmpty(productName))
                        {
                            productName = productName.Trim().ToLower();
                            products = products.Where(
                                p => p.ProductName.ToLower().Contains(productName) || 
                                p.categoryName.ToLower().Contains(productName) || 
                                p.subCategoryName.ToLower().Contains(productName) 
                            ).ToList();
                        }

                        if(sortByName != 0)
                        {
                            products = sortByName >= 1 ? products.OrderBy(p => p.ProductName).ToList() : products.OrderByDescending(p => p.ProductName).ToList();
                        }

                        if (sortByPrice != 0)
                        {
                            products = sortByPrice >= 1 ? products.OrderBy(p => p.Price).ToList() : products.OrderByDescending(p => p.Price).ToList();
                        }

                        int totalPages = (int)Math.Ceiling((double)products.Count / pageSize);

                        //Phân trang
                        products = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Lấy thành công tất cả sản phẩm!",
                            Data = new
                            {
                                products = products!,
                                totalPages = totalPages!,
                            }
                        };
                    }
                }
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
    
        public BaseResponseModel Put( ProductRequesModule req )
        {
            if(req.IsValid())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_UpdateProduct", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int) { Value = req.ProductId });
                            cmd.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar) { Value = req.ProductName });
                            cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.NVarChar) { Value = req.Image });
                            cmd.Parameters.Add(new SqlParameter("@CategoryID", SqlDbType.Int) { Value = req.CategoryId });
                            cmd.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.Int) { Value = req.Supplier });
                            cmd.Parameters.Add(new SqlParameter("@SubCategoryID", SqlDbType.Int) { Value = req.SubCategoryId });
                            cmd.Parameters.Add(new SqlParameter("@ExpiryDate", SqlDbType.DateTime) { Value = req.ExpiryDate });
                            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar) { Value = req.Description });
                            cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = req.Price });

                            try
                            {
                                int rowAffected = cmd.ExecuteNonQuery();

                                return new BaseResponseModel()
                                {
                                    IsSuccess = rowAffected > 0 ? true : false,
                                    Message = rowAffected > 0 ? "Cập nhật sản phẩm thành công..!" : "Cập nhật thất bại. Vui lòng kiểm tra lại..!",
                                };
                            }
                            catch (Exception ex)
                            {
                                return new BaseResponseModel()
                                {
                                    IsSuccess = false,
                                    Message = $"Lỗi trong quá trình thực thi: {ex}"
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
                Message = "Vui lòng kiểm tra lại thông tin..!"
            };
        }

        public BaseResponseModel Post(ProductRequestPostModule req)
        {
            if (req.IsValid())
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_InsertProduct", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar) { Value = req.ProductName });
                            cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.NVarChar) { Value = req.Image });
                            cmd.Parameters.Add(new SqlParameter("@CategoryID", SqlDbType.Int) { Value = req.CategoryId });
                            cmd.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.Int) { Value = req.Supplier });
                            cmd.Parameters.Add(new SqlParameter("@SubCategoryID", SqlDbType.Int) { Value = req.SubCategoryId });
                            cmd.Parameters.Add(new SqlParameter("@ExpiryDate", SqlDbType.DateTime) { Value = req.ExpiryDate });
                            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar) { Value = req.Description });
                            cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = req.Price });

                            try
                            {
                                int rowAffected = cmd.ExecuteNonQuery();

                                return new BaseResponseModel()
                                {
                                    IsSuccess = true,
                                    Message = rowAffected > 0 ? "Thêm sản phẩm thành công..!" : "Thêm thất bại. Vui lòng kiểm tra lại..!",
                                };
                            }
                            catch (Exception ex)
                            {
                                return new BaseResponseModel()
                                {
                                    IsSuccess = false,
                                    Message = $"Lỗi trong quá trình thực thi: {ex}"
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
                Message = "Vui lòng kiểm tra lại thông tin..!"
            };
        }

        public BaseResponseModel GetProductsModerator(
            int? productId,
            int? cateId,
            int? subCateId,
            int? supplierId,
            string? productName,
            int pageNumber = 1,
            int pageSize = 8,
            int sortByName = 0 /*1: tăng, -1: giảm, 0:*/,
            int sortByPrice = 0 /*1: tăng, -1: giảm, 0:*/
        ) {
            List<ProductViewUserResponseModel> products = new List<ProductViewUserResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var command = new SqlCommand("SP_GetAllProductsModerator", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Thực hiện lệnh
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductViewUserResponseModel product = new ProductViewUserResponseModel
                                {
                                    ProductId = Convert.ToInt32(reader["product_id"]),
                                    ProductName = reader["product_name"] as string ?? string.Empty,
                                    Image = Convert.ToString(reader["image"]),
                                    TotalQuantity = reader["totalQuantity"] != DBNull.Value ? Convert.ToInt32(reader["totalQuantity"]) : 0,
                                    CategoryId = Convert.ToInt32(reader["Category_id"]),
                                    Supplier = Convert.ToInt32(reader["Supplier"]),
                                    SubCategoryId = Convert.ToInt32(reader["SubCategoryID"]),
                                    ExpriryDate = Convert.ToDateTime(reader["ExpriryDate"]),
                                    Description = Convert.ToString(reader["Description"]),
                                    ModifiedBy = Convert.ToString(reader["ModifiedBy"]),
                                    CreateTime = Convert.ToDateTime(reader["CreateTime"]),
                                    ModifiedTime = Convert.ToDateTime(reader["ModifiedTime"]),
                                    DeleteTime = reader["DeleteTime"] != DBNull.Value ? Convert.ToDateTime(reader["DeleteTime"]) : null,
                                    Price = Convert.ToDecimal(reader["price"]),
                                    priceHistoryId = Convert.ToInt32(reader["priceHistoryId"]),
                                    categoryName = Convert.ToString(reader["category_name"]) as string ?? string.Empty,
                                    subCategoryName = Convert.ToString(reader["SubCategoryName"]) as string ?? string.Empty
                                };

                                products.Add(product);
                            }
                        }

                        //check if product_id has values
                        if (productId.HasValue)
                        {
                            products = products.Where(p => p.ProductId.Equals(productId)).ToList();
                        }

                        //check if subCateId has value
                        if (subCateId.HasValue)
                        {
                            products = products.Where(p => p.SubCategoryId == subCateId).ToList();
                        }

                        //check if cate_id has value
                        if (cateId.HasValue)
                        {
                            products = products.Where(p => p.CategoryId == cateId).ToList();
                        }

                        if (supplierId.HasValue)
                        {
                            products = products.Where(p => p.Supplier == supplierId).ToList();
                        }

                        //check if productName has value
                        if (!string.IsNullOrEmpty(productName))
                        {
                            productName = productName.Trim().ToLower();
                            products = products.Where(
                                p => p.ProductName.ToLower().Contains(productName) ||
                                p.categoryName.ToLower().Contains(productName) ||
                                p.subCategoryName.ToLower().Contains(productName)
                            ).ToList();
                        }

                        if (sortByName != 0)
                        {
                            products = sortByName >= 1 ? products.OrderBy(p => p.ProductName).ToList() : products.OrderByDescending(p => p.ProductName).ToList();
                        }

                        if (sortByPrice != 0)
                        {
                            products = sortByPrice >= 1 ? products.OrderBy(p => p.Price).ToList() : products.OrderByDescending(p => p.Price).ToList();
                        }

                        int totalPages = (int)Math.Ceiling((double)products.Count / pageSize);

                        //Phân trang
                        products = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Lấy thành công tất cả sản phẩm!",
                            Data = new
                            {
                                products = products!,
                                totalPages = totalPages!,
                            }
                        };
                    }
                }
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

        public BaseResponseModel Delete(int? productId)
        {
            if (productId.HasValue)
            {
                try
                {
                    using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand("SP_DeleteProduct", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            // Thêm tham số cần thiết
                            cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId });

                            int rowAffected = cmd.ExecuteNonQuery();

                            return new BaseResponseModel()
                            {
                                IsSuccess = rowAffected > 0 ? true : false,
                                Message = rowAffected > 0 ? "Xóa sản phẩm thành công..!" : "Xóa thất bại. Vui lòng kiểm tra lại..!",
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
                Message = "Kiểm tra lại thông tin..!"
            };
        }
    }
}
