using BLL.Interface;
using BLL.LoginBLL;
using DTO.District;
using DTO.Responses;
using System.Data.SqlClient;

namespace BLL
{
    public class DistrictBLL : IDistrict
    {
        public BaseResponseModel Gets()
        {
            List<DistrictResponseModel> communeResponseModel = new List<DistrictResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetDistrict", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Thực hiện lệnh
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DistrictResponseModel model = new DistrictResponseModel()
                                {
                                    DistrictId = reader.GetInt32(0),
                                    DistrictName = reader.GetString(1),
                                    ProvinceId = reader.GetInt32(2),
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

        public BaseResponseModel GetDistrictByProvinceID(int provinceID)
        {
            List<DistrictResponseModel> districtResponseModel = new List<DistrictResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetDistrictByProvinceID", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProvinceID", provinceID);

                        // Thực hiện lệnh
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DistrictResponseModel model = new DistrictResponseModel()
                                {
                                    DistrictId = reader.GetInt32(0),
                                    DistrictName = reader.GetString(1),
                                    ProvinceId = reader.GetInt32(2),
                                };
                                districtResponseModel.Add(model);
                            }
                        }
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Data = districtResponseModel,
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
      
    }
}
