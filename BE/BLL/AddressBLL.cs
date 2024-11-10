using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Address;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class AddressBLL : IAddress
    {
        private readonly ICommune commune;
        public AddressBLL(ICommune commune)
        {
            this.commune = commune;
        }
        public BaseResponseModel GetById(int iD)
        {
            AddressResponseModel res = new AddressResponseModel();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetAddressById", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AddressId", iD);
                        // Thực hiện lệnh
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                res = new AddressResponseModel()
                                {
                                    AddressId = reader.GetInt32(0),
                                    CommuneId = reader.GetInt32(1),
                                    HouseNumber = reader.GetString(2),
                                    Note = reader.GetString(3)
                                };
                            }
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Thành Công!",
                        Data = res
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
    
        public BaseResponseModel Post(AddressRequestModule req)
        {
            if (req.CommuneId == 0)
            {
                //--insert Commune
                req.CommuneId = (int)(commune.PostCommune(new DTO.Commune.CommuneRequestModule()
                {
                    CommuneName = req.CommuneName,
                    DistrictId = req.DistrictId
                }).Data ?? 0);
            }
            //--insert Address
            try
            {
                using(var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("InsertAddress", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //tham số đầu vào
                        cmd.Parameters.AddWithValue("@CommuneID", req.CommuneId);
                        cmd.Parameters.AddWithValue("@HouseNumber", req.HouseNumber);
                        cmd.Parameters.AddWithValue("@Note", req.Note);

                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            int insertedId = 0;

                            if (reader.Read()) {
                                insertedId = reader.GetInt32(0);
                            }

                            return new BaseResponseModel() {
                                IsSuccess = true,
                                Message = "Thành Công",
                                Data = insertedId
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

        public BaseResponseModel GetAddressID(AddressRequestModule request)
        {
            int addressID = 0;

            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("GetAddressID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số đầu vào
                        cmd.Parameters.AddWithValue("@CommuneID", request.CommuneId);
                        cmd.Parameters.AddWithValue("@HouseNumber", request.HouseNumber);
                        cmd.Parameters.AddWithValue("@Note", request.Note);

                        conn.Open();

                        // Sử dụng SqlDataReader để đọc kết quả trả về
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                addressID = reader.GetInt32(0);
                            }
                        }

                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = $"Thành Công!",
                    Data = addressID
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex}",
                    Data = addressID
                };
            }
        }

        public BaseResponseModel GetAddressString()
        {
            List<KeyValuePair<int, string>> address = new List<KeyValuePair<int, string>>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("SP_GetFullAddress", conn))
                    {
                        cmd.CommandType= CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                address.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                            }
                        }
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = $"Thành Công!",
                    Data = address
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex}",
                };
            }
        }

    }
}
