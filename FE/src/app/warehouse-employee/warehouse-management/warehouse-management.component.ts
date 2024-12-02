import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { BaseResponseModel, BaseResponseModule } from '../../shared/module/base-response/base-response.module';
import { Warehouse, ContructorWarehouse } from '../warehouse-employee.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
@Component({
  selector: 'app-warehouse-management',
  standalone: true,
  imports: [CommonModule, NotificationComponent],
  templateUrl: './warehouse-management.component.html',
  styleUrls: ['./warehouse-management.component.css'],
  
})
export class WarehouseManagementComponent{

  selectWarehouse: Warehouse = ContructorWarehouse();
  isShowDelete: boolean = false;
  flagDelete: boolean = false;

  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  warehouses: Warehouse[] = [];

  ngOnInit(): void {
    this.getWarehouses();
  }

  constructor( private routes: Router, private warehouseEmployeeService: WarehouseEmployeeService) {
    console.log('123')
        // Đảm bảo console.log và console.error luôn là một hàm hợp lệ
        console.log = console.log || function (...args: any[]) { /* Do nothing */ };
        console.error = console.error || function (...args: any[]) { /* Do nothing */ };
  }

  async getWarehouses() {
    try {
      const result: BaseResponseModel = await this.warehouseEmployeeService.getWarehouses();
      if (result.isSuccess) {
        this.warehouses = result.data;
      } else {
        console.error('Không thể lấy dữ liệu:', result.message);
      }
    } catch (error) {
      console.error('Lỗi khi lấy dữ liệu:', error);
    }
  }

  // async onShowDeleteWareHouse(warehouseID: number) {
  //   const response: BaseResponseModel = await this.warehouseEmployeeService.deleteWarehouse(warehouseID);
    
  //   if(response.isSuccess) {
  //     this.dataNotification.status = 'success',
  //     this.dataNotification.messages = response.message!;
  //     await this.getWarehouses();
  //   } else {
  //     this.dataNotification.status = 'warning',
  //     this.dataNotification.messages = response.message!;      
  //   }
  //   this.trigger = new Date();
  // }

  async onShowDeleteWareHouse(warehouseID: number) {
    try {
      // Gọi API xóa kho
      const response: BaseResponseModel = await this.warehouseEmployeeService.deleteWarehouse(warehouseID);
  
      // Kiểm tra trạng thái phản hồi
      if (response.isSuccess) {
        this.dataNotification = {
          status: 'success',
          messages: response.message || 'Xóa thành công!',
        };
        await this.getWarehouses(); // Cập nhật danh sách kho sau khi xóa
      } else {
        this.dataNotification = {
          status: 'warning',
          messages: response.message || 'Không thể xóa kho!',
        };
      }
  
      // Kích hoạt lại giao diện nếu cần
      this.trigger = new Date();
    } catch (error) {
      // Xử lý lỗi nếu API thất bại
      this.dataNotification = {
        status: 'error',
        messages: 'Có lỗi xảy ra khi thực hiện xóa kho!',
      };
      console.error('Error deleting warehouse:', error);
    }
  }

  // async onShowDeleteWareHouse(warehouseID: number) {
  //   try {
  //     const response: BaseResponseModel = await this.warehouseEmployeeService.deleteWarehouse(warehouseID);

  //     if (response.isSuccess) {
  //       this.dataNotification = {
  //         status: 'success',
  //         messages: response.message || 'Xóa kho thành công!',
  //       };
  //       await this.getWarehouses();
  //     } else {
  //       this.dataNotification = {
  //         status: 'warning',
  //         messages: response.message || 'Không thể xóa kho!',
  //       };
  //     }
  //   } catch (error) {
  //     this.dataNotification = {
  //       status: 'error',
  //       messages: 'Có lỗi xảy ra khi thực hiện xóa kho!',
  //     };
  //     console.error(error);
  //   }
  // }
  
  
}
