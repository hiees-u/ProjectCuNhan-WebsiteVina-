using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO;
using DTO.Order;
using DTO.Responses;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Data;
using System.Data.SqlClient;


namespace BLL
{
    public class OrderBLL : IOrder
    {
        private readonly IUser iUser;

        public OrderBLL(IUser user) {
            this.iUser = user;
        }
        public BaseResponseModel Post(OrderRequestModule request)
        {
            try
            {
                if (!request.IsValid())
                {
                    return new BaseResponseModel()
                    {
                        IsSuccess = false,
                        Message = "Nhập đủ thông tin!"
                    };
                }

                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertOrderWithDetails", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@Phone", System.Data.SqlDbType.NVarChar, 11) { Value = request.phone });
                        command.Parameters.Add(new SqlParameter("@Address_ID", System.Data.SqlDbType.Int) { Value = request.addressId });
                        command.Parameters.Add(new SqlParameter("@Name_Recipient", System.Data.SqlDbType.NVarChar, 50) { Value = request.namerecipient });
                        command.Parameters.Add(new SqlParameter("@PaymentStatus", SqlDbType.Bit) { Value = request.paymendStatus });

                        SqlParameter tvpParam = command.Parameters.AddWithValue("@ProductQuantities", CreateProductQuantityDataTable(request.products));
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dbo.ProductQuantityType";

                        conn.Open();

                        command.ExecuteNonQuery();
                    }
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Đặt hàng thành công!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình Thêm Đơn Đặt Hàng: {ex}"
                };
            }
        }

        private DataTable CreateProductQuantityDataTable(List<ProductQuantity> productQuantities)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PriceHistoryId", typeof(int));
            dt.Columns.Add("Quantity", typeof(int));

            foreach (var pq in productQuantities)
            {
                dt.Rows.Add(pq.PriceHistoryId, pq.Quantity);
            }
            return dt;
        }

        public BaseResponseModel Get(int orderState)
        {
            if (orderState < -1 || orderState > 4)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = "Lỗi tham số truyền vào [0,1,2,3,4]"
                };
            }

            List<OrderResponseModelv2> repon = new List<OrderResponseModelv2>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SP_GetOrderDetailsByState", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@OrderState", orderState));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    int columnIndex = reader.GetOrdinal("Trạng Thái Thanh Toán");
                                    bool paymentStatus = !reader.IsDBNull(columnIndex) && reader.GetBoolean(columnIndex);
                                }
                                catch (IndexOutOfRangeException ex)
                                {
                                    Console.WriteLine("Cột 'Trạng Thái Thanh Toán' không tồn tại: " + ex.Message);
                                }

                                repon.Add(new OrderResponseModelv2()
                                {
                                    productname = reader.IsDBNull(reader.GetOrdinal("product_name")) ? null : reader.GetString(reader.GetOrdinal("product_name")),
                                    image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")),
                                    price = reader.IsDBNull(reader.GetOrdinal("price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("price")),
                                    quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    totalprice = reader.IsDBNull(reader.GetOrdinal("TỔNG TIỀN")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TỔNG TIỀN")),
                                    state = reader.IsDBNull(reader.GetOrdinal("Trạng Thái")) ? 0 : reader.GetInt32(reader.GetOrdinal("Trạng Thái")),
                                    orderid = reader.IsDBNull(reader.GetOrdinal("Mã Đơn Hàng")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mã Đơn Hàng")),
                                    pricehistoryid = reader.IsDBNull(reader.GetOrdinal("Mã Giá")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mã Giá")),
                                    paymentStatus = !reader.IsDBNull(reader.GetOrdinal("Trạng Thái Thanh Toán")) && reader.GetBoolean(reader.GetOrdinal("Trạng Thái Thanh Toán"))
                                });

                            }
                        }
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Lấy Thành Công!",
                    Data = repon
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình Lấy Đơn Đặt Hàng: {ex}"
                };
            }
        }

        public BaseResponseModel Delete(int OrderId, int PriceHistory)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SP_DeleteOrderDetailState", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                        command.Parameters.Add(new SqlParameter("@PriceHistoryId", PriceHistory));

                        string resultMessage = (string)command.ExecuteScalar();

                        //command.ExecuteNonQuery();
                    }
                }
                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Xóa Chi Tiết Đơn Hàng Thành Công!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình Lấy Đơn Đặt Hàng: {ex}"
                };
            }
        }
        public BaseResponseModel UpdatePaymentStatus(string orderId, bool isPaid)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SP_UpdatePaymentStatus", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@OrderId", orderId));
                        command.Parameters.Add(new SqlParameter("@PaymentStatus", isPaid));

                        command.ExecuteNonQuery();
                    }
                }

                return new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Cập nhật trạng thái thanh toán thành công!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi khi cập nhật trạng thái thanh toán: {ex.Message}"
                };
            }
        }

        public BaseResponseModel UpdateOrder(Order order)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var command = new SqlCommand("SP_UpdatePaymentStatus", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));
                        command.Parameters.Add(new SqlParameter("@PaymentStatus", order.State = 1));

                        command.ExecuteNonQuery();
                    }
                }
                return new BaseResponseModel { IsSuccess = true, Message = "Cập nhật đơn hàng thành công!" };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel { IsSuccess = false, Message = $"Lỗi khi cập nhật đơn hàng: {ex.Message}" };
            }
        }

        public BaseResponseModel CreateOrder(Order order)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var command = new SqlCommand("SP_CreateOrder", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@TotalPayment", order.TotalPayment));
                        command.Parameters.Add(new SqlParameter("@State", order.State));
                        command.Parameters.Add(new SqlParameter("@CreateAt", order.CreateAt));

                        // Lấy OrderId sau khi thêm
                        var orderIdParam = new SqlParameter("@OrderId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(orderIdParam);

                        command.ExecuteNonQuery();

                        // Gắn OrderId tự động sinh
                        order.OrderId = (int)orderIdParam.Value;
                    }
                }

                return new BaseResponseModel { IsSuccess = true, Message = "Tạo đơn hàng thành công!", Data = order };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel { IsSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //get order warehouse employee
        public BaseResponseModel GetByOrderApprover()
        {
            try
            {
                List<OrderResponseModelv3> lst = new List<OrderResponseModelv3>();
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_getOrderOrderApprover", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderResponseModelv3 res = new OrderResponseModelv3()
                                {
                                    orderId = Convert.ToInt32(reader["Mã đơn hàng"]),
                                    phone = reader["SDT nhận hàng"].ToString()!,
                                    nameRecip = reader["Tên người nhận"].ToString()!,
                                    total = Convert.ToDecimal(reader["Tổng tiền"]),
                                    created = Convert.ToDateTime(reader["Thời gian đặt"]),
                                    createBy = reader["Người đặt"].ToString()!,
                                    address = reader["Address"].ToString()!
                                };
                                lst.Add(res);
                            }
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Lấy danh sách đơn đặt hàng thành công..!",
                        Data = lst
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = "Đã xảy ra lỗi..!"
                };
            }
        }

        public BaseResponseModel GetOrderDetailByOA(int oID)
        {
            try
            {
                List<OrderDetailResponseModel> lst = new List<OrderDetailResponseModel>();
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_getOrderDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@OrderId", oID));

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderDetailResponseModel od = new OrderDetailResponseModel()
                                {
                                    priceHistory = Convert.ToInt32(reader["PriceHistory"]),
                                    Name = reader["Name"].ToString(),
                                    Image = reader["Image"].ToString(),
                                    Gia = Convert.ToDecimal(reader["Gia"]),
                                    SoLuongTon = reader["SoLuongTon"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongTon"]) : 0,
                                    SoLuongMua = Convert.ToInt32(reader["SoLuongMua"])
                                };
                                lst.Add(od);
                            }
                        }
                    }
                    return new BaseResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Lấy danh sách chi tiết đơn đặt hàng thành công..!",
                        Data = lst
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = "Đã xảy ra lỗi..!"
                };
            }
        }

        public string GenerateInvoice(Invoice invoice)
        {
            try
            {
                string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\BE"));
                string filePath = Path.Combine(projectDirectory, "Invoices", $"{invoice.customerName}_{Guid.NewGuid()}.pdf");

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (PdfWriter writer = new PdfWriter(fs))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            var document = new iText.Layout.Document(pdf);

                            string boldFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "timesbd.ttf");
                            PdfFont boldFont = PdfFontFactory.CreateFont(boldFontPath, PdfEncodings.IDENTITY_H);
                            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

                            // Bảng 2 cột
                            var headerTable = new iText.Layout.Element.Table(new float[] { 1, 1 });
                            headerTable.SetWidth(UnitValue.CreatePercentValue(100));

                            // Cột trái: Tiêu đề căn giữa + thông tin công ty căn trái
                            var leftCell = new iText.Layout.Element.Cell()
                                .Add(new Paragraph("CÔNG TY CÀ PHÊ VINA")
                                    .SetFont(boldFont)
                                    .SetFontSize(16)
                                    //.SetBold()
                                    .SetTextAlignment(TextAlignment.CENTER))
                                .Add(new Paragraph("Địa chỉ: 140 Lê Trọng Tấn, Phường Tây Thạnh, Quận Tân Phú, TP.HCM")
                                    .SetFont(font)
                                    .SetFontSize(12)
                                    .SetMaxWidth(UnitValue.CreatePercentValue(100))
                                    .SetTextAlignment(TextAlignment.LEFT))
                                .Add(new Paragraph("ĐT: 0283 8163 318")
                                    .SetFont(font)
                                    .SetFontSize(12)
                                    .SetTextAlignment(TextAlignment.LEFT));
                            leftCell.SetBorder(Border.NO_BORDER);

                            // Cột phải: Tiêu đề căn giữa + nội dung hóa đơn căn trái
                            var rightCell = new iText.Layout.Element.Cell()
                                .Add(new Paragraph("HÓA ĐƠN BÁN HÀNG")
                                    .SetFont(boldFont)
                                    .SetFontSize(16)
                                    //.SetBold()
                                    .SetTextAlignment(TextAlignment.CENTER))
                                .Add(new Paragraph("Kinh doanh cà phê đóng hộp")
                                    .SetFont(font)
                                    .SetFontSize(12)
                                    .SetTextAlignment(TextAlignment.LEFT));
                            rightCell.SetBorder(Border.NO_BORDER);

                            headerTable.AddCell(leftCell);
                            headerTable.AddCell(rightCell);

                            // Thêm bảng header vào tài liệu
                            document.Add(headerTable);

                            // Thông tin khách hàng
                            document.Add(new Paragraph($"Tên khách hàng: {invoice.customerName}")
                                .SetFont(font)
                                .SetFontSize(12));

                            document.Add(new Paragraph($"Địa chỉ: {invoice.customerAddress}")
                                .SetFont(font)
                                .SetFontSize(12));

                            // Bảng chi tiết
                            var table = new iText.Layout.Element.Table(new float[] { 1, 4, 2, 2, 2 });
                            table.SetWidth(UnitValue.CreatePercentValue(100));

                            table.AddHeaderCell(new Paragraph("TT").SetFont(font));
                            table.AddHeaderCell(new Paragraph("TÊN HÀNG").SetFont(font));
                            table.AddHeaderCell(new Paragraph("SỐ LƯỢNG").SetFont(font));
                            table.AddHeaderCell(new Paragraph("ĐƠN GIÁ").SetFont(font));
                            table.AddHeaderCell(new Paragraph("THÀNH TIỀN").SetFont(font));

                            BaseResponseModel model = GetOrderDetailByOA(invoice.OrderId);

                            List<OrderDetailResponseModel> items =
                                model.Data as List<OrderDetailResponseModel> ?? new List<OrderDetailResponseModel>();

                            for (int i = 0; i < items.Count; i++) // Tạo 15 dòng để giống mẫu
                            {
                                var item = items[i];
                                table.AddCell(new Paragraph((i + 1).ToString()).SetFont(font));
                                table.AddCell(new Paragraph(item.Name).SetFont(font));
                                table.AddCell(new Paragraph(item.SoLuongMua.ToString()).SetFont(font));
                                table.AddCell(new Paragraph(FormatCurrency.formatCurrency(item.Gia)).SetFont(font));
                                table.AddCell(new Paragraph(FormatCurrency.formatCurrency(item.SoLuongMua * item.Gia)).SetFont(font));
                            }

                            // Thêm dòng Tổng Cộng
                            var totalRow = new iText.Layout.Element.Cell(1, 2)
                                .Add(new Paragraph("TỔNG CỘNG")
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.CENTER));
                            //totalRow.SetBorderTop(new SolidBorder(1));
                            table.AddCell(totalRow);

                            int totalQuantity = items.Sum(x => x.SoLuongMua);

                            table.AddCell(new Paragraph(totalQuantity.ToString())
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.CENTER));
                            //.SetBorderTop(new SolidBorder(1)))

                            table.AddCell(new Paragraph("")
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.CENTER));
                                //.SetBorderTop(new SolidBorder(1)))

                            decimal totalAmount = items.Sum(x => x.SoLuongMua * x.Gia);

                            table.AddCell(new Paragraph(FormatCurrency.formatCurrency(totalAmount))
                                .SetFont(boldFont)
                                .SetTextAlignment(TextAlignment.CENTER));
                                //.SetBorderTop(new SolidBorder(1)));

                            document.Add(table);

                            // Chuyển số thành chữ
                            string amountInWords = FormatCurrency.NumberToWords(totalAmount);

                            // Dòng ghi chú "Thành tiền (viết bằng chữ)"
                            document.Add(new Paragraph($"Thành tiền (viết bằng chữ): {amountInWords}")
                                .SetFont(font)
                                .SetFontSize(12));

                            // Tạo bảng 2 cột cho Ngày tháng và chữ ký
                            iText.Layout.Element.Table footerTable = new iText.Layout.Element.Table(new float[] { 1, 1 });  
                            // 1 cột cho KHÁCH HÀNG, 1 cột cho ngày tháng và người bán
                            footerTable.SetWidth(UnitValue.CreatePercentValue(100));

                            // Cột bên trái: Dòng trống và KHÁCH HÀNG căn giữa
                            footerTable.AddCell(new iText.Layout.Element.Cell()
                                .Add(new Paragraph("")  // Dòng trống
                                .SetFont(font)
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.CENTER))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  // Không viền

                            // Cột bên phải: Ngày tháng và tên người bán căn giữa
                            footerTable.AddCell(new iText.Layout.Element.Cell()
                                .Add(new Paragraph($"Hồ Chí Minh, Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}")
                                .SetFont(font)
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.CENTER))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  // Không viền

                            footerTable.AddCell(new iText.Layout.Element.Cell()
                                .Add(new Paragraph("KHÁCH HÀNG")
                                .SetFont(font)
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.CENTER))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  // Không viền


                            footerTable.AddCell(new iText.Layout.Element.Cell()
                                .Add(new Paragraph("NGƯỜI BÁN HÀNG")
                                .SetFont(font)
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.CENTER))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  // Không viền

                            footerTable.AddCell(new iText.Layout.Element.Cell()
                                .Add(new Paragraph("")  // Dòng trống
                                .SetFont(font)
                                .SetFontSize(30)
                                .SetTextAlignment(TextAlignment.CENTER))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  //

                            footerTable.AddCell(new iText.Layout.Element.Cell()
                                .Add(new Paragraph("")  // Dòng trống
                                .SetFont(font)
                                .SetFontSize(30)
                                .SetTextAlignment(TextAlignment.CENTER))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  //

                            footerTable.AddCell(new iText.Layout.Element.Cell()
                                .Add(new Paragraph("")  // Dòng trống
                                .SetFont(font)
                                .SetFontSize(20)
                                .SetTextAlignment(TextAlignment.CENTER))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  //

                            BaseResponseModel rs = iUser.GetFullName();
                            string sellerName = string.Empty;

                            if (rs.IsSuccess)
                            {
                                if (rs.Data != null) { sellerName = Convert.ToString(rs.Data); }
                            }


                            footerTable.AddCell(new iText.Layout.Element.Cell()
                            .Add(new Paragraph(sellerName)  // Tên người bán
                            .SetFont(font)
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.CENTER))
                            .SetBorder(iText.Layout.Borders.Border.NO_BORDER));  // Không viền

                            document.Add(footerTable);
                        }
                    }
                }
                return filePath;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error generating PDF: " + ex.Message);
                throw;
            }
        }

        //cập nhật trạng thái thành 2 chuyển sang bên kho đóng gói
        public BaseResponseModel UpdateStateOrderByOA(int orderID)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SP_updateStateOrder2", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@OrderId", orderID));
                        try
                        {
                            command.ExecuteNonQuery();

                            return new BaseResponseModel()
                            {
                                IsSuccess = true,
                                Message = "Đổi trạng thái đơn hàng thành công.!"
                            };
                        }
                        catch (Exception ex)
                        {
                            return new BaseResponseModel()
                            {
                                IsSuccess = false,
                                Message = $"Lỗi khi cập nhật đơn hàng",
                                Data = ex.Message
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseModel { IsSuccess = false, Message = $"Lỗi khi cập nhật đơn hàng", Data = ex.Message };
            }
        }

    }
}

