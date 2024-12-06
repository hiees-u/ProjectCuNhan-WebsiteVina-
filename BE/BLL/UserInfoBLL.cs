﻿using BLL.Interface;
using BLL.LoginBLL;
using DTO.Responses;
using DTO.UserInfo;
using System.Data.SqlClient;

namespace BLL
{
    public class UserInfoBLL : IUserInfo
    {
        private readonly IAddress _address;

        public UserInfoBLL(IAddress address)
        {
            _address = address;
        }

        public BaseResponseModel Get()
        {
            try
            {
                UserInfoResponseModel uf = new UserInfoResponseModel();
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetUserInfoByUserName", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader()) 
                        { 
                            if (reader.Read()) 
                            {
                                uf = new UserInfoResponseModel
                                {
                                    accountName = reader["Tên Đăng Nhập"]?.ToString() ?? string.Empty,
                                    fullName = reader["Họ Tên"]?.ToString() ?? string.Empty,
                                    email = reader["Email"]?.ToString() ?? string.Empty,
                                    phone = reader["Số Điện Thoại"]?.ToString() ?? string.Empty,
                                    gender = reader["Giới Tính"] != DBNull.Value ? (reader["Giới Tính"] as int?).GetValueOrDefault() : -1,
                                    customerType = reader["Loại Khách Hàng"]?.ToString(),
                                    address = reader["Địa Chỉ"]?.ToString() ?? string.Empty,
                                    addressId = reader["Địa Chỉ ID"] != DBNull.Value ? (reader["Địa Chỉ ID"] as int?).GetValueOrDefault() : 0,
                                    commune = reader["Xã"] != DBNull.Value ? (reader["Xã"] as int?).GetValueOrDefault() : 0,
                                    province = reader["Tỉnh"] != DBNull.Value ? (reader["Tỉnh"] as int?).GetValueOrDefault() : 0,
                                    district = reader["Quận"] != DBNull.Value ? (reader["Quận"] as int?).GetValueOrDefault() : 0,
                                };
                            }
                        }
                    }
                }
                return new BaseResponseModel
                {
                    Data = uf,
                    IsSuccess = true,
                    Message = "Lấy Thành Công!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy Thông Tin Khách Hàng: {ex}"
                };
            }
        }

        public BaseResponseModel Put(UserInfoRequestModel req)
        {

            //--kiểm tra req.CommuneName, req.HouseNumber, req.Note đã tồn tại trong bảng Địa Chỉ chưa
            req.addressId = (int)((this._address.GetAddressID(new DTO.Address.AddressRequestModule()
            {
                //CommuneName = req.CommuneName,
                HouseNumber = req.houseNumber,
                CommuneId = req.commune,
                Note = req.note
            })).Data!); //-- trả về  0 hoăc AddressId

            //--> chưa tồn tại địa chỉ --> insert mới
            if (req.addressId <= 0) {
                req.addressId = (int)(this._address.Post(new DTO.Address.AddressRequestModule()
                {
                    CommuneName = req.communeName,
                    CommuneId = req.commune,
                    HouseNumber = req.houseNumber,
                    Note = req.note,
                    DistrictId = req.districtId,
                })).Data!;
            }


            //-- update UF
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateUserInfo", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //- thêm các tham số đầu vào
                        cmd.Parameters.AddWithValue("@FullName", req.fullName);
                        cmd.Parameters.AddWithValue("@Phone", req.phone);
                        cmd.Parameters.AddWithValue("@Email", req.email);
                        cmd.Parameters.AddWithValue("@AddressID", req.addressId);
                        cmd.Parameters.AddWithValue("@Gender", req.gender);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return new BaseResponseModel()
                        {
                            IsSuccess = true,
                            Message = "Cập Nhật Thành Công!"
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
 
        public BaseResponseModel GetAccontName()
       
        {
            try
            {
                string accountName = string.Empty;
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetUserAccountName", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                accountName = reader["AccountName"].ToString() ?? string.Empty;
                            }
                        }
                    }
                }
                return new BaseResponseModel
                {
                    Data = accountName,
                    IsSuccess = true,
                    Message = "Lấy Thành Công!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy Thông Tin Khách Hàng: {ex}"
                };
            }
        }
    }
}
