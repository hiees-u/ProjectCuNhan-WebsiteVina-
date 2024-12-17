using BLL.Interface;
using BLL.LoginBLL;
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
                Console.WriteLine($"Month: {Month}, Year: {Year}"); // Log kiểm tra tham số
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
                                    Day = Convert.ToInt32(reader["Day"]),
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
                                    Month = Convert.ToInt32(reader["Month"]),
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

        public BaseResponseModel GetDailySalesReportByCustomerTye(DateTime InputDate)
        {
            try
            {
                List<DailyReportCustomerTypeResponseModel> resultList = new List<DailyReportCustomerTypeResponseModel>();

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("GetRevenueByCustomerTypeInDay", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Date", InputDate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultList.Add(new DailyReportCustomerTypeResponseModel()
                                {
                                    InputDate = InputDate,
                                    CustomerType = reader["CustomerType"].ToString(),
                                    Revenue = Convert.ToDecimal(reader["Revenue"])
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

        public BaseResponseModel GetRangeSalesReportByCustomerTye(DateTime StartDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }

        public BaseResponseModel GetMonthSalesReportByCustomerTye(int Month, int Year)
        {
            throw new NotImplementedException();
        }

        public BaseResponseModel GetYearSalesReportByCustomerTye(int Year)
        {
            throw new NotImplementedException();
        }
    }
}
