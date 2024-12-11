using BLL.Interface;
using BLL.LoginBLL;
using DTO.DeliveryNote;
using DTO.Responses;
using DTO.WareHouse;
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
        public BaseResponseModel GetOrderDetail(int orderID)
        {
            List<OrderDetailsResponseModel> listOrderdetail = new List<OrderDetailsResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetOrderDetailsWE", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderID", orderID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderDetailsResponseModel OrderDetail = new OrderDetailsResponseModel()
                                {
                                    ProductId = Convert.ToInt32(reader["product_id"]),
                                    ProductName = reader["product_name"] as string ?? string.Empty,
                                    Quantity = Convert.ToInt32(reader["order_quantity"]),
                                    CellId = Convert.ToInt32(reader["CellID"]),
                                    CellName = reader["CellName"] as string ?? string.Empty,
                                    WarehouseId = Convert.ToInt32(reader["WarehouseID"]),
                                    WarehouseName = reader["WarehouseName"] as string ?? string.Empty,
                                    PriceHistoryId = Convert.ToInt32(reader["priceHistoryId"]),
                                };
                                listOrderdetail.Add(OrderDetail);
                            }
                        }
                    }
                }

                BaseResponseModel response = new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Success!",
                    Data = listOrderdetail
                };

                if (listOrderdetail.Count == 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Đơn hàng chưa được tạo!!";
                }

                return response;

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin đơn hàng: {ex}"
                };
            }
        }

        public BaseResponseModel GetOrderIDs()
        {
            List<OrderIDsResponseModel> listOrderID = new List<OrderIDsResponseModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionStringHelper.Get()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetOrderIDs", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderIDsResponseModel OrderIDs = new OrderIDsResponseModel()
                                {
                                    OrderId = Convert.ToInt32(reader["Order_ID"]),
                                };
                                listOrderID.Add(OrderIDs);
                            }
                        }
                    }
                }

                BaseResponseModel response = new BaseResponseModel()
                {
                    IsSuccess = true,
                    Message = "Success!",
                    Data = listOrderID
                };

                if (listOrderID.Count < 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Đơn hàng chưa được tạo!!";
                }

                return response;

            }
            catch (Exception ex)
            {
                return new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = $"Lỗi trong quá trình lấy thông tin đơn hàng: {ex}"
                };
            }
        }

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
