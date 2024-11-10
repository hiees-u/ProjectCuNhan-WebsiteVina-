using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class SupplierBLL : ISupplier
    {
        public BaseResponseModel GetById(int supplierID)
        {
            Supplier supplier = new Supplier();
            try
            {
                using (var connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.GetSupplierById(@SupplierID)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                supplier = new Supplier
                                {
                                    SupplierId = (int)reader["SupplierID"],
                                    SupplierName = reader["SupplierName"].ToString(),
                                    AddressId = (int)reader["AddressId"],
                                    DeleteTime = reader["DeleteTime"] != DBNull.Value ? (DateTime?)reader["DeleteTime"] : null,
                                    DeleteBy = reader["DeleteBy"] != DBNull.Value ? reader["DeleteBy"].ToString() : null,
                                };
                            } 
                        }
                    }
                }
                if (supplier != null)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Thành Công",
                        Data = supplier,
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
                    Message = $"Lỗi trong quá trình lấy tất cả sản phẩm: {ex}"
                };
            }
        }
    }
}
