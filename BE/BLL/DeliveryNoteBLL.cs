using BLL.Interface;
using BLL.LoginBLL;
using DTO.DeliveryNote;
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
    public class DeliveryNoteBLL : IDeliveryNote
    {
        public BaseResponseModel InsertDeliveryNote(int warehouseID, string note, List<DeliveryOrderDetai> deliveryOrderDetails)
        {
            var response = new BaseResponseModel();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_ExportWarehouseGoodsByOrder", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                        cmd.Parameters.AddWithValue("@Note", note);

                        // Create the table-valued parameter for receipt details
                        DataTable deliveryNoteDetailsTable = new DataTable();
                        deliveryNoteDetailsTable.Columns.Add("OrderID", typeof(int));
                        deliveryNoteDetailsTable.Columns.Add("priceHistoryId", typeof(int));
                        deliveryNoteDetailsTable.Columns.Add("Quantity", typeof(int));
                        deliveryNoteDetailsTable.Columns.Add("CellID", typeof(int));

                        foreach (var detail in deliveryOrderDetails)
                        {
                            deliveryNoteDetailsTable.Rows.Add(detail.OrderID, detail.PriceHistoryId, detail.Quantity, detail.CellID);
                        }

                        SqlParameter receiptDetailsParam = new SqlParameter("@OrderDetails", SqlDbType.Structured)
                        {
                            TypeName = "dbo.DeliveryOrderDetailType",  // The type you created in SQL Server
                            Value = deliveryNoteDetailsTable
                        };
                        cmd.Parameters.Add(receiptDetailsParam);

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        response.IsSuccess = true;
                        response.Message = "Phiếu xuất kho đã được thêm thành công!";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Lỗi trong quá trình thêm phiếu xuất kho: {ex.Message}";
            }
            return response;
        }
    }
}
