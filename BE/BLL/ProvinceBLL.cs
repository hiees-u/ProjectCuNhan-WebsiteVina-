using BLL.Interface;
using BLL.LoginBLL;
using DTO.Provinces;
using DTO.Responses;
using System.Data.SqlClient;

namespace BLL
{
    public class ProvinceBLL : IProvince
    {
        public BaseResponseModel Gets()
        {
            List<ProvincesResponseModel> provincesResponseModels = new List<ProvincesResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetProvinces", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Thực hiện lệnh
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProvincesResponseModel model = new ProvincesResponseModel()
                                {
                                    ProvinceId = Convert.ToInt32(reader["ProvinceID"]),
                                    ProvinceName = reader["ProvinceName"].ToString() ?? string.Empty,
                                };
                                provincesResponseModels.Add(model);
                            }
                        }
                    }
                }
                return new BaseResponseModel() {
                    IsSuccess = true,
                    Data = provincesResponseModels,
                    Message = "Thành Công"
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
    }
}
