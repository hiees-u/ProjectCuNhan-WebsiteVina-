import { Component, EventEmitter, Output } from '@angular/core';
import { WarehouseEmployeeService } from '../warehouse-employee.service';

import {
  Warehouse, OrderIDsResponse, ContructorOrderIDsResponseModule,
  OrderDetailResponse, ContructorOrderDetailResponseModule
}
  from '../warehouse-employee.module';

import { BaseResponseModel, BaseResponseModule }
  from '../../shared/module/base-response/base-response.module';

import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { ExportWarehouseRequest, ContructorRequestExportWarehouseModule }
  from '../warehouse-employee.module';

import { from } from 'rxjs';
@Component({
  selector: 'app-out-warehouse',
  imports: [FormsModule, CommonModule],
  templateUrl: './out-warehouse.component.html',
  styleUrl: './out-warehouse.component.css'
})
export class OutWarehouseComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private warehouseEmployeeService: WarehouseEmployeeService) { }

  ngOnInit(): void {
    this.getWarehouses();
    this.getOrderIDs();
  }
  exportWarehouse: ExportWarehouseRequest = ContructorRequestExportWarehouseModule();
  isAdd: boolean = false;

  warehouses: Warehouse[] = [];
  orderIds: OrderIDsResponse[] = [];
  products: OrderDetailResponse[] = [];
  selectedOrderId: number = 0;

  async getWarehouses() {
    const data = await this.warehouseEmployeeService.getWarehouses();
    this.warehouses = data.data;

  }
  async getOrderIDs() {
    const data = await this.warehouseEmployeeService.getOrderIds();
    this.orderIds = data.data;
  }

  async onOrderChange(event: Event) {
    // Kiểm tra sự tồn tại của event.target và ép kiểu cho chính xác
    const target = event.target as HTMLSelectElement;
    if (target) {
      const orderId = Number(target.value); // Chuyển đổi giá trị thành số
      this.selectedOrderId = orderId; // Lưu ID đơn hàng được chọn

      if (orderId > 0) {
        try {
          // Gọi API để lấy chi tiết sản phẩm của đơn hàng
          const data = await this.warehouseEmployeeService.getOrderDetails(orderId);
          console.error('Dữ liệu lấy từ chi tiết đơn đặt hàng: ', data);
          // Kiểm tra dữ liệu trả về từ API
          if (data && data.data && Array.isArray(data.data)) {
            console.error('aaaa', data.data)
            this.products = data.data.map((product: OrderDetailResponse) => ({
              ...product, // Sử dụng tất cả thuộc tính từ `OrderDetailResponse`
              exportQuantity: product.quantity ?? 0, // Đảm bảo giá trị mặc định nếu `quantityOrder` không tồn tại
            }));
            console.error(this.products)
          } else {
            console.warn('API trả về dữ liệu không hợp lệ:', data);
            this.products = []; // Làm rỗng danh sách sản phẩm nếu dữ liệu không đúng
          }
        } catch (error) {
          console.error('Lỗi khi lấy chi tiết đơn hàng:', error);
          this.products = []; // Xóa danh sách sản phẩm trong trường hợp lỗi
        }
      } else {
        this.products = []; // Xóa danh sách sản phẩm nếu `orderId` không hợp lệ
      }
    }
  }

  async submitForm() {
    // Kiểm tra kho hàng đã được chọn hay chưa
    if (!this.exportWarehouse.warehouseId) {
      alert('Vui lòng chọn kho hàng!');
      return;
    }

    // Kiểm tra danh sách sản phẩm có hợp lệ hay không
    if (!this.products || this.products.length === 0) {
      alert('Danh sách sản phẩm trống. Vui lòng kiểm tra lại!');
      return;
    }

    // Chuẩn bị chi tiết xuất kho
    const exportDetails = this.products.map(product => ({
      orderID: this.selectedOrderId, // Thêm orderId từ đơn hàng được chọn
      priceHistoryId: product.priceHistoryId, // Sử dụng thuộc tính từ `OrderDetailResponse`
      quantity: product.quantity, // Đảm bảo sử dụng đúng field
      cellID: product.cellId || 0 // Gán giá trị mặc định nếu `cellId` không được chọn
    }));

    console.error('Luu orderDetail bang tam: ', exportDetails);

    // Kiểm tra nếu có sản phẩm chưa nhập ô lưu trữ hoặc số lượng không hợp lệ
    const invalidProducts = exportDetails.filter(detail => detail.cellID === 0 || detail.quantity <= 0);
    if (invalidProducts.length > 0) {
      alert('Có sản phẩm chưa chọn ô lưu trữ hoặc số lượng xuất không hợp lệ. Vui lòng kiểm tra lại!');
      return;
    }

    // Chuẩn bị dữ liệu gửi
    const exportData: ExportWarehouseRequest = {
      warehouseId: this.exportWarehouse.warehouseId,
      note: this.exportWarehouse.note || '', // Gán giá trị mặc định nếu ghi chú trống
      deliveryNoteDetail: exportDetails
    };

    console.error('Luu Nhap Kho: ', exportData);
    try {
      // Gửi yêu cầu API
      const response = await this.warehouseEmployeeService.exportGoodsByOrder(exportData);

      if (response && response.isSuccess) {
        alert('Lưu phiếu xuất kho thành công!');
        this.isClose.emit(true); // Đóng form và thông báo hoàn tất
      } else {
        alert('Có lỗi xảy ra. Vui lòng thử lại!');
      }
    } catch (error) {
      console.error('Lỗi khi gửi yêu cầu xuất kho:', error);
      alert('Không thể lưu phiếu xuất kho. Vui lòng thử lại sau!');
    }
  }
}
