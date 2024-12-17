using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Cells;
using DTO.Product;
using DTO.Responses;
using DTO.WareHouse;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class CellsBLL : ICell
    {
        public BaseResponseModel Delete(int cellID)
        {
            var response = new BaseResponseModel();

            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("DeleteCell", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CellID", cellID);
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        conn.Open();

                        cmd.ExecuteNonQuery();

                        string resultMessage = messageParam.Value.ToString();

                        if (resultMessage == "Xóa thành công")
                        {
                            response.IsSuccess = true;
                            response.Message = resultMessage;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = resultMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Lỗi trong quá trình xóa Ô: {ex.Message}";
            }

            return response;
        }


        public BaseResponseModel GetAllProducts()
        {
            List<ProductsResponseModel> listProduct = new List<ProductsResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetProductList", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductsResponseModel productsResponseModel = new ProductsResponseModel()
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"] as string ?? string.Empty
                                };
                                listProduct.Add(productsResponseModel);
                            }
                        }
                    }
                }

                BaseResponseModel response = new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Success!",
                    Data = listProduct
                };

                if (listProduct.Count < 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Không có sản phẩm nào!!!";
                }

                return response;

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin sản phẩm!!!: {ex}"
                };
            }
        }

        public BaseResponseModel GetCellByShelve(int shelveID)
        {
            List<CellsProductByShelveResponseModel> productList = new List<CellsProductByShelveResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("GetProductByShelveID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add the input parameter for ShelvesID
                        cmd.Parameters.AddWithValue("@ShelvesID", shelveID);

                        // Add parameter OUTPUT to receive notifications from the procedure
                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Execute the command and read the result
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Add each shelf to the list
                                productList.Add(new CellsProductByShelveResponseModel()
                                {
                                    CellId = reader.GetInt32(0),
                                    CellName = reader.GetString(1),
                                    Quantity = reader.GetInt32(2),
                                    ProductId = reader.GetInt32(3),
                                    ProductName = reader.GetString(4),
                                    Image = !reader.IsDBNull(5) ? reader.GetString(5) : null,
                                    TotalQuantity = reader.GetInt32(6),
                                    ExpriryDate = !reader.IsDBNull(7) ? reader.GetDateTime(7).Date : (DateTime?)null,
                                    ModifiedBy = reader.GetString(8),
                                    CreateTime = !reader.IsDBNull(9) ? reader.GetDateTime(9) : DateTime.MinValue,
                                    ModifiedTime = !reader.IsDBNull(10) ? reader.GetDateTime(10) : (DateTime?)null,
                                    DeleteTime = !reader.IsDBNull(11) ? reader.GetDateTime(11) : (DateTime?)null
                                });
                            }
                        }

                        // Return the response with data and message from OUTPUT
                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = messageParam.Value.ToString(),
                            Data = productList // Return the list  product in cell of shelves
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin sản phẩm trong ô của kệ trong kho: {ex.Message}"
                };
            }
        }

        public BaseResponseModel GetInfoProductsByProductID(int productId)
        {
            List<InfoProductsResponseModel> productOfWarehouseList = new List<InfoProductsResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("GetInfoProductsByProductID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add the input parameter for warehouseID
                        cmd.Parameters.AddWithValue("@product_id", productId);

                        // Add parameter OUTPUT to receive notifications from the procedure
                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Execute the command and read the result
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Add each shelf to the list
                                productOfWarehouseList.Add(new InfoProductsResponseModel()
                                {
                                    ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                                    Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")),
                                    WarehouseName = reader.GetString(reader.GetOrdinal("WarehouseName")),
                                    CellName = reader.GetString(reader.GetOrdinal("CellName")),
                                    ShelvesName = reader.GetString(reader.GetOrdinal("ShelvesName")),
                                    TotalQuantity = reader.IsDBNull(reader.GetOrdinal("totalQuantity")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TotalQuantity")),
                                    ExpriryDate = reader.IsDBNull(reader.GetOrdinal("ExpriryDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpriryDate"))
                                });
                            }
                        }

                        // Return the response with data and message from OUTPUT
                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = messageParam.Value.ToString(),
                            Data = productOfWarehouseList // Return the list  product in warehouse
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin sản phẩm trong kho: {ex.Message}"
                };
            }
        }

        public BaseResponseModel GetProductsByWarehouseID(int warehouseID)
        {
            List<ProductOfWarehouseResponseModel> productOfWarehouseList = new List<ProductOfWarehouseResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("GetProductsByWarehouseID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add the input parameter for warehouseID
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                        // Add parameter OUTPUT to receive notifications from the procedure
                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Execute the command and read the result
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Add each shelf to the list
                                productOfWarehouseList.Add(new ProductOfWarehouseResponseModel()
                                {
                                    WarehouseName = reader.GetString(0),
                                    ShelvesName = reader.GetString(1),
                                    CellName = reader.GetString(2),
                                    ProductName = reader.GetString(3),
                                    Image = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                                    Quantity = reader.GetInt32(5),
                                    TotalQuantity = reader.GetInt32(6),
                                    ExpriryDate = !reader.IsDBNull(7) ? reader.GetDateTime(7).Date : (DateTime?)null,
                                    CellModifiedBy = reader.GetString(8),
                                    CellCreateTime = !reader.IsDBNull(9) ? reader.GetDateTime(9) : DateTime.MinValue,
                                    CellModifiedTime = !reader.IsDBNull(10) ? reader.GetDateTime(10) : (DateTime?)null,
                                    CellDeleteTime = !reader.IsDBNull(11) ? reader.GetDateTime(11) : (DateTime?)null
                                });
                            }
                        }

                        // Return the response with data and message from OUTPUT
                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = messageParam.Value.ToString(),
                            Data = productOfWarehouseList // Return the list  product in warehouse
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin sản phẩm trong kho: {ex.Message}"
                };
            }
        }

        public BaseResponseModel GetProductsExpriryDate()
        {
            List<ProductViewUserResponseModel> listProductsExpriryDate = new List<ProductViewUserResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetProductsExpiringInNextMonth", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductViewUserResponseModel productsExpriryDateResponseModel = new ProductViewUserResponseModel()
                                {
                                    ProductId = Convert.ToInt32(reader["product_id"]),
                                    ProductName = reader["product_name"] as string ?? string.Empty,
                                    Image = reader["image"] as string ?? string.Empty,
                                    ModifiedBy = reader["ModifiedBy"] as string ?? string.Empty,
                                    ExpriryDate = reader.IsDBNull(reader.GetOrdinal("ExpriryDate")) ? (DateTime?)null : Convert.ToDateTime(reader["ExpriryDate"]),
                                    CreateTime = (DateTime)reader["CreateTime"],
                                    ModifiedTime = reader.IsDBNull(reader.GetOrdinal("ModifiedTime")) ? (DateTime?)null : Convert.ToDateTime(reader["ModifiedTime"])
                                };
                                listProductsExpriryDate.Add(productsExpriryDateResponseModel);
                            }
                        }
                    }
                }

                BaseResponseModel response = new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Success!",
                    Data = listProductsExpriryDate
                };

                if (listProductsExpriryDate.Count < 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Không có sản phẩm nào hết hạn!!!";
                }

                return response;

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin sản phẩm hết hạn!!!: {ex}"
                };
            }
        }

        public BaseResponseModel Post(CellPostRequestModule request)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("AddCell", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CellName", request.CellName);
                        cmd.Parameters.AddWithValue("@ShelvesID", request.ShelvesId);
                        cmd.Parameters.AddWithValue("@Quantity", request.Quantity);
                        cmd.Parameters.AddWithValue("@product_id", request.ProductId);
                        
                        // Add parameter OUTPUT to receive notifications from the procedure
                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Get alert from parameter OUTPUT
                        string message = messageParam.Value.ToString();

                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = message
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình thêm ô: {ex.Message}"
                };
            }
        }

        public BaseResponseModel Put(CellRequestModule request)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("UpdateCell", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CellID", request.CellId);
                        cmd.Parameters.AddWithValue("@CellName", request.CellName);
                        cmd.Parameters.AddWithValue("@ShelvesID", request.ShelvesId);
                        cmd.Parameters.AddWithValue("@Quantity", request.Quantity);
                        cmd.Parameters.AddWithValue("@product_id", request.ProductId);
                        

                        // Add parameter OUTPUT
                        var outputMessage = new SqlParameter("@OutputMessage", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMessage);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Get alert from parameter OUTPUT
                        string message = outputMessage.Value.ToString();

                        return new BaseResponseModel()
                        {
                            IsSuccess = message.Contains("Thành công"),
                            Message = message
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình cập nhật ô: {ex.Message}"
                };
            }
        }
    }
}
