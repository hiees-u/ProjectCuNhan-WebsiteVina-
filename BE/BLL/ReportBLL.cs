using BLL.Interface;
using BLL.LoginBLL;
using DTO.Address;
using DTO.Order;
using DTO.Report;
using DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class ReportBLL : IReport
    {
        public BaseResponseModel GetDailySalesReport(DateTime InputDate)
        {
            try
            {
                List<DailyReportResponseModel> resultList = new List<DailyReportResponseModel>();

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetDailySalesReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@InputDate", InputDate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultList.Add(new DailyReportResponseModel()
                                {
                                    InputDate = InputDate,
                                    ProductId = reader["MaSanPham"].ToString(),
                                    ProductName = reader["TenSanPham"].ToString(),
                                    TotalSales = Convert.ToInt32(reader["TongSoLuongBan"]),
                                    TotalRevenue = Convert.ToDecimal(reader["TongDoanhThu"])
                                });
                            }
                        }
                    }
                }

                if (resultList.Count == 0)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Không có dữ liệu!",
                        Data = null
                    };
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Thành Công!",
                    Data = resultList
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex.Message}",
                    Data = null
                };
            }
        }

        public BaseResponseModel GetMonthlySalesReport(int Month, int Year)
        {
            try
            {
                List<MonthlyReportResponseModel> resultList = new List<MonthlyReportResponseModel>();

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetMonthlySalesReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Month", Month);
                        cmd.Parameters.AddWithValue("@Year", Year);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultList.Add(new MonthlyReportResponseModel()
                                {
                                    Month = Convert.ToInt32(reader["Thang"]),
                                    Year = Convert.ToInt32(reader["Nam"]),
                                    ProductId = reader["MaSanPham"].ToString(),
                                    ProductName = reader["TenSanPham"].ToString(),
                                    TotalSales = Convert.ToInt32(reader["TongSoLuongBanRa"]),
                                    TotalRevenue = Convert.ToDecimal(reader["TongDoanhThu"])
                                });
                            }
                        }
                    }
                }

                if (resultList.Count == 0)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Không có dữ liệu!",
                        Data = null
                    };
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Thành Công!",
                    Data = resultList
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex.Message}",
                    Data = null
                };
            }
        }

        public BaseResponseModel GetWeeklySalesReport(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                List<WeeklyReportResponseModel> resultList = new List<WeeklyReportResponseModel>();

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetWeeklySalesReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultList.Add(new WeeklyReportResponseModel()
                                {
                                    StartDate = StartDate,
                                    EndDate = EndDate,
                                    ProductId = reader["MaSanPham"].ToString(),
                                    ProductName = reader["TenSanPham"].ToString(),
                                    TotalSales = Convert.ToInt32(reader["TongSoLuongBanRa"]),
                                    TotalRevenue = Convert.ToDecimal(reader["TongDoanhThu"])
                                });
                            }
                        }
                    }
                }

                if (resultList.Count == 0)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Không có dữ liệu!",
                        Data = null
                    };
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Thành Công!",
                    Data = resultList
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex.Message}",
                    Data = null
                };
            }
        }

        public BaseResponseModel GetYearlySalesReport(int Year)
        {
            try
            {
                List<YearlyReportResponseModel> resultList = new List<YearlyReportResponseModel>();

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SP_GetSalesReportInYear", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Year", Year);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultList.Add(new YearlyReportResponseModel()
                                {
                                    Year = Convert.ToInt32(reader["Nam"]),
                                    ProductId = reader["MaSanPham"].ToString(),
                                    ProductName = reader["TenSanPham"].ToString(),
                                    TotalSales = Convert.ToInt32(reader["TongSoLuongBanRa"]),
                                    TotalRevenue = Convert.ToDecimal(reader["TongDoanhThu"])
                                });
                            }
                        }
                    }
                }

                if (resultList.Count == 0)
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Không có dữ liệu!",
                        Data = null
                    };
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Thành Công!",
                    Data = resultList
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
