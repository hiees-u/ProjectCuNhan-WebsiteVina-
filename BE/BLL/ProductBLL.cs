using BLL.Interface;
using BLL.LoginBLL;
using DTO.Product;
using DTO.Responses;
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
            int pageSize = 10,
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

                        //Phân trang
                        products = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Lấy thành công tất cả sản phẩm!",
                            Data = products
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
    }
}
