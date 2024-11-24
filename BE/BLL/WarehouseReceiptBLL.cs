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
