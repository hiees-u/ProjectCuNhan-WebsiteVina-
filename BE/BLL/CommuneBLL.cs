using BLL.Interface;
using BLL.LoginBLL;
using DTO.Commune;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class CommuneBLL : ICommune
    {
        public BaseResponseModel Gets()
        {
            List<CommuneResponseModel> communeResponseModel = new List<CommuneResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetCommunes", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Thực hiện lệnh
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommuneResponseModel model = new CommuneResponseModel()
                                {
                                    CommuneId = reader.GetInt32(0),
                                    CommuneName = reader.GetString(1),
                                    DistrictId = reader.GetInt32(2),
                                };
                                communeResponseModel.Add(model);
                            }
                        }
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Data = communeResponseModel,
                    Message = "Thành Công"
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

        public BaseResponseModel GetCommunesByDistrictIDAsync(int districID) {
            List<CommuneResponseModel> communeResponseModel = new List<CommuneResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetCommunesByDistrictID", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DistrictID", districID);

                        // Thực hiện lệnh
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommuneResponseModel model = new CommuneResponseModel()
                                {
                                    CommuneId = reader.GetInt32(0),
                                    CommuneName = reader.GetString(1),
                                    DistrictId = reader.GetInt32(2),
                                };
                                communeResponseModel.Add(model);
                            }
                        }
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Data = communeResponseModel,
                    Message = "Thành Công"
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

        public BaseResponseModel PostCommune(CommuneRequestModule req)
        {
            if (req.isValid())
            {
                try
                {
                    using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                    {
                        using (var cmd = new SqlCommand("InsertCommune", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm các tham số đầu vào
                            cmd.Parameters.AddWithValue("@CommuneName", req.CommuneName);
                            cmd.Parameters.AddWithValue("@DistrictID", req.DistrictId);

                            conn.Open();

                            // Sử dụng SqlDataReader để đọc kết quả trả về
                            using (var reader = cmd.ExecuteReader())
                            {
                                int insertedID = 0;

                                // Đọc kết quả trả về và lấy ID
                                if (reader.Read())
                                {
                                    insertedID = reader.GetInt32(0);
                                }

                                return new BaseResponseModel()
                                {
                                    IsSuccess = true,
                                    Message = "Thành Công!",
                                    Data = insertedID
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
            else
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = "Không Được Để Trống!"
                };
            }
        }
    }
}
