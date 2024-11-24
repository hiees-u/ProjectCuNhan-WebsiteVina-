using BLL.Interface;
using BLL.LoginBLL;
using DTO.Responses;
using DTO.Shevle;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class ShelveBLL: IShelve
    {
        public BaseResponseModel Delete(int shelveID)
        {
            var response = new BaseResponseModel();

            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("DeleteShelve", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ShelvesID", shelveID);
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
                response.Message = $"Lỗi trong quá trình xóa kệ: {ex.Message}";
            }

            return response;
        }

        public BaseResponseModel GetShelveOfWarehousehouse(int warehouseID)
        {
            List<ShelveResponseModel> shelvesList = new List<ShelveResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("GetShelveByWarehouseID", conn))
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
                                shelvesList.Add(new ShelveResponseModel()
                                {
                                    ShelvesId = reader.GetInt32(0),
                                    ShelvesName = reader.GetString(1),
                                    WarehouseId = reader.GetInt32(2),
                                    ModifiedBy = reader.GetString(3),
                                    CreateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : DateTime.MinValue,
                                    ModifiedTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                                    DeleteTime = !reader.IsDBNull(6) ? reader.GetDateTime(6) : (DateTime?)null
                                });
                            }
                        }

                        // Return the response with data and message from OUTPUT
                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = messageParam.Value.ToString(),
                            Data = shelvesList // Return the list of shelves
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin kệ của kho: {ex.Message}"
                };
            }
        }


        public BaseResponseModel Post(ShelvePostRequestModule request)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("AddShelves", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ShelvesName", request.ShelvesName);
                        cmd.Parameters.AddWithValue("@WarehouseID", request.WarehouseId);
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
                    Message = $"Lỗi trong quá trình thêm kệ: {ex.Message}"
                };
            }
        }

        public BaseResponseModel Put(ShelveRequestModule request)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("UpdateShelve", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ShelvesID", request.ShelvesId);
                        cmd.Parameters.AddWithValue("@ShelvesName", request.ShelvesName);
                        cmd.Parameters.AddWithValue("@WarehouseID", request.WarehouseId);

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
                    Message = $"Lỗi trong quá trình cập nhật kệ: {ex.Message}"
                };
            }
        }
    }
}
