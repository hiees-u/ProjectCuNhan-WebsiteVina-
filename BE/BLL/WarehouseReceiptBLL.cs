using BLL.Interface;
using BLL.LoginBLL;
using DLL.Models;
using DTO.Responses;
using DTO.WarehouseReceipt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WarehouseReceiptBLL : IWarehouseReceipt
    {
        public BaseResponseModel Delete(int WarehouseReceiptID)
        {
            var response = new BaseResponseModel(); 
            try { 
                using (var conn = new SqlConnection(ConnectionStringHelper.Get())) 
                { 
                    using (var cmd = new SqlCommand("sp_DeleteWarehouseReceipt", conn)) 
                    { 
                        cmd.CommandType = CommandType.StoredProcedure; 
                        cmd.Parameters.AddWithValue("@WarehouseReceiptID", WarehouseReceiptID); 
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 100) 
                        { 
                            Direction = ParameterDirection.Output 
                        };

                        cmd.Parameters.Add(messageParam); conn.Open(); 
                        cmd.ExecuteNonQuery(); 

                        string resultMessage = messageParam.Value.ToString(); 
                        response.IsSuccess = resultMessage.StartsWith("Xóa thành công"); 
                        response.Message = resultMessage; 
                    } 
                } 
            } catch (Exception ex) { 
                response.IsSuccess = false; response.Message = $"Lỗi trong quá trình xóa kho: {ex.Message}"; 
            }
            return response;
        }

        public BaseResponseModel GetWarehouseReceiptInfo(int WarehouseReceiptID)
        {
            var response = new BaseResponseModel();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("sp_GetWarehouseReceiptInfo", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WarehouseReceiptID", WarehouseReceiptID);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            var warehouseReceipt = new
                            {
                                Header = new WarehouseReceiptInfo(),
                                Details = new List<ReceiptDetail>()
                            };

                            bool hasHeader = false;

                            // Đọc dữ liệu phiếu nhập kho (Header)
                            if (reader.Read())
                            {
                                hasHeader = true;
                                warehouseReceipt.Header.WarehouseReceiptID = reader.GetInt32(reader.GetOrdinal("WarehouseReceiptID"));
                                warehouseReceipt.Header.EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID"));
                                warehouseReceipt.Header.CreateAt = reader.GetDateTime(reader.GetOrdinal("CreateAt"));
                                warehouseReceipt.Header.WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID"));
                            }

                            // Nếu không có header, kết thúc và trả về lỗi
                            if (!hasHeader)
                            {
                                response.IsSuccess = false;
                                response.Message = "Không tìm thấy phiếu nhập kho với ID đã cung cấp.";
                                return response;
                            }

                            // Chuyển sang kết quả tiếp theo để đọc chi tiết phiếu nhập kho
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var detail = new ReceiptDetail
                                    {
                                        ProductID = reader.GetInt32(reader.GetOrdinal("product_id")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                        CellID = reader.GetInt32(reader.GetOrdinal("CellID")),
                                        PurchaseOrderId = reader.GetInt32(reader.GetOrdinal("PurchaseOrderID")),
                                    };
                                    warehouseReceipt.Details.Add(detail);
                                }
                            }

                            response.IsSuccess = true;
                            response.Data = warehouseReceipt;
                            response.Message = "Lấy thông tin phiếu nhập kho thành công.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Lỗi trong quá trình lấy thông tin phiếu nhập kho: {ex.Message}";
            }

            return response;
        }

        public BaseResponseModel GetWarehouseReceiptsByWarehouse(int warehouseID)
        {
            var response = new BaseResponseModel();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    using (var cmd = new SqlCommand("sp_GetWarehouseReceiptsByWarehouse", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            var receipts = new List<WarehouseReceiptOverview>();

                            while (reader.Read())
                            {
                                var receipt = new WarehouseReceiptOverview
                                {
                                    WarehouseReceiptID = reader.GetInt32(reader.GetOrdinal("WarehouseReceiptID")),
                                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                    CreateAt = reader.GetDateTime(reader.GetOrdinal("CreateAt")),
                                    WarehouseName = reader.GetString(reader.GetOrdinal("WarehouseName"))
                                };

                                receipts.Add(receipt);
                            }

                            if (receipts.Count == 0)
                            {
                                response.IsSuccess = false;
                                response.Message = "Không tìm thấy phiếu nhập nào cho kho đã cung cấp.";
                                return response;
                            }

                            response.IsSuccess = true;
                            response.Data = receipts;
                            response.Message = "Lấy thông tin các phiếu nhập kho thành công.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Lỗi trong quá trình lấy thông tin phiếu nhập kho: {ex.Message}";
            }

            return response;
        }

        public BaseResponseModel InsertWarehouseReceipt(int warehouseID, List<ReceiptDetail> receiptDetails)
        {
            var response = new BaseResponseModel();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_InsertWarehouseReceipt", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);

                        // Create the table-valued parameter for receipt details
                        DataTable receiptDetailsTable = new DataTable();
                        receiptDetailsTable.Columns.Add("ProductID", typeof(int));
                        receiptDetailsTable.Columns.Add("Quantity", typeof(int));
                        receiptDetailsTable.Columns.Add("CellID", typeof(int));
                        receiptDetailsTable.Columns.Add("PurchaseOrderID", typeof(int));

                        foreach (var detail in receiptDetails)
                        {
                            receiptDetailsTable.Rows.Add(detail.ProductID, detail.Quantity, detail.CellID, detail.PurchaseOrderId);
                        }

                        SqlParameter receiptDetailsParam = new SqlParameter("@ReceiptDetails", SqlDbType.Structured)
                        {
                            TypeName = "dbo.ReceiptDetailType",  // The type you created in SQL Server
                            Value = receiptDetailsTable
                        };
                        cmd.Parameters.Add(receiptDetailsParam);

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        response.IsSuccess = true;
                        response.Message = "Phiếu nhập kho đã được thêm thành công!";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Lỗi trong quá trình thêm phiếu nhập kho: {ex.Message}";
            }
            return response;
        }
    }
}
