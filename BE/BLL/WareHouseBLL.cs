using BLL.Interface;
using BLL.LoginBLL;
using DTO.Responses;
using DTO.WareHouse;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class WareHouseBLL: IWareHouse
    {
        public BaseResponseModel Delete(int warehouseId)
        {
            var response = new BaseResponseModel();

            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("DeleteWarehouse", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseId);
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
                response.Message = $"Lỗi trong quá trình xóa kho: {ex.Message}";
            }

            return response;
        }

        public BaseResponseModel Get()
        {
            List<WareHouseResponseModel> listWareHouse = new List<WareHouseResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllWarehouses", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WareHouseResponseModel warehouseResponseModel = new WareHouseResponseModel()
                                {
                                    WarehouseId = Convert.ToInt32(reader["WarehouseID"]),
                                    WarehouseName = reader["WarehouseName"] as string ?? string.Empty,
                                    Address = Convert.ToInt32(reader["AddressID"]),
                                    ModifiedBy = reader["ModifiedBy"] as string ?? string.Empty,
                                    CreateTime = (DateTime)(reader.IsDBNull(reader.GetOrdinal("CreateTime")) ? (DateTime?)null : Convert.ToDateTime(reader["CreateTime"])),
                                    ModifiedTime = reader.IsDBNull(reader.GetOrdinal("ModifiedTime")) ? (DateTime?)null : Convert.ToDateTime(reader["ModifiedTime"]),
                                    DeleteTime = reader.IsDBNull(reader.GetOrdinal("DeleteTime")) ? (DateTime?)null : Convert.ToDateTime(reader["DeleteTime"])

                                };
                                listWareHouse.Add(warehouseResponseModel);
                            }
                        }
                    }
                }

                BaseResponseModel response = new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Success!",
                    Data = listWareHouse
                };

                if (listWareHouse.Count < 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Kho chưa được tạo!!";
                }

                return response;

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin kho: {ex}"
                };
            }
        }

        public BaseResponseModel GetWareHouseID(int warehouseID)
        {
            WareHouseResponseModel res = new WareHouseResponseModel();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("GetWarehouseByID", conn))
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
                            if (reader.Read())
                            {
                                res = new WareHouseResponseModel()
                                {
                                    WarehouseId = reader.GetInt32(0),
                                    WarehouseName = reader.GetString(1),
                                    Address = reader.GetInt32(2),
                                    ModifiedBy = reader.GetString(3),
                                    CreateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : DateTime.MinValue,
                                    ModifiedTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                                    DeleteTime = !reader.IsDBNull(6) ? reader.GetDateTime(6) : (DateTime?)null
                                };
                            }
                        }

                        // Return the response with data and message from OUTPUT
                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = messageParam.Value.ToString(),
                            Data = res
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin kho: {ex.Message}"
                };
            }
        }

        public BaseResponseModel Post(WareHousePostRequestModule request)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("AddWarehouse", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@WarehouseName", request.WarehouseName);
                        cmd.Parameters.AddWithValue("@AddressID", request.AddressId);
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
                            Message = message // return alert from procedure
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình thêm kho: {ex.Message}"
                };
            }
        }

        public BaseResponseModel Put(WareHouseRequestModule request)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("UpdateWarehouse", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@WarehouseID", request.WarehouseId);
                        cmd.Parameters.AddWithValue("@WarehouseName", request.WarehouseName);
                        cmd.Parameters.AddWithValue("@AddressID", request.AddressId);                        

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
                    Message = $"Lỗi trong quá trình cập nhật kho: {ex.Message}"
                };
            }
        }
    }
}
