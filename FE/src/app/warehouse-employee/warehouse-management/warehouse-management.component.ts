import { AddWarehouseComponent } from './../add-warehouse/add-warehouse.component';
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { BaseResponseModel, BaseResponseModule } from '../../shared/module/base-response/base-response.module';
import { Warehouse, ContructorWarehouse, WareHouseRequestWarehouseEmployee, ContructorRequestWarehouseModule } from '../warehouse-employee.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { FormsModule } from '@angular/forms';
import { ViewWarehouseComponent } from '../view-warehouse/view-warehouse.component';
@Component({
  selector: 'app-warehouse-management',
  // standalone: true,
  // RouterOutlet,
  imports: [CommonModule,
    AddWarehouseComponent,
    FormsModule,
    NotificationComponent,
    ViewWarehouseComponent
  ],
  templateUrl: './warehouse-management.component.html',
  styleUrls: ['./warehouse-management.component.css'],

})
export class WarehouseManagementComponent {

  isShowAddWarehouuse: boolean | undefined;
  flag: boolean = false;

  selectWarehouse: Warehouse = ContructorWarehouse();
  isShowDelete: boolean = false;
  flagDelete: boolean = false;

  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  warehouses: Warehouse[] = [];

  selectWarehouseUpdate: WareHouseRequestWarehouseEmployee =
    ContructorRequestWarehouseModule();
  isShowDetail: boolean = false;
  flagDetail: boolean = false;

  @Input() searchTextIn: string = '';
  searchText: string = '';

  ngOnInit(): void {
    this.getWarehouses();
  }

  constructor(private router: Router, private warehouseEmployeeService: WarehouseEmployeeService) {
  }

  onShowAddWarehouse() {
    this.flag = true;
    this.isShowAddWarehouuse = true;
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

  async onShowDeleteWareHouse(warehouseID: number) {
    // try {
    // Gọi API xóa kho
    const response: BaseResponseModel = await this.warehouseEmployeeService.deleteWarehouse(warehouseID);

    // Kiểm tra trạng thái phản hồi
    if (response.isSuccess) {
      this.dataNotification = {
        messages: response.message!,
        status: 'success',
      };

      await this.getWarehouses(); // Cập nhật danh sách kho sau khi xóa

    } else {
      this.dataNotification = {
        messages: response.message!,
        status: 'error',
      };
    }
    // Kích hoạt lại giao diện nếu cần
    this.trigger = new Date();
  }

  handleAddWareHouse() {
    this.flag = true;
    this.isShowAddWarehouuse = true;
  }

  logOutHandler() {
    this.router.navigate(['/login']);
    localStorage.removeItem('token');
  }

  handleCloseAdd(is: boolean) {
    this.isShowAddWarehouuse = false;
    this.flag = false;
    this.getWarehouses();
  }

  onChangeSelectedWarehouse(selectWarehouse: WareHouseRequestWarehouseEmployee) {

    // console.error('Thong tin KHO Truyen Qua',selectWarehouse);
    this.selectWarehouseUpdate = selectWarehouse;
    this.isShowDetail = true;
    this.flagDetail = true;
  }
  handleCloseDetail(is: boolean) {
    this.isShowDetail = !is;
    this.flagDetail = false;
    this.getWarehouses();
  }
  onChangeSearch(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value;
  }
  onSearch() {

  }
  // async handleSearchWarehouse() {
  //   const searchText = this.searchTextIn?.trim();

  //   if (!searchText) {
  //     await this.getWarehouses();
  //     return;
  //   }

  //   const result: BaseResponseModel = await this.warehouseEmployeeService.getWarehousesByName(searchText);
  //   if (result.isSuccess) {
  //     this.warehouses = result.data;
  //     this.dataNotification = {
  //       messages: result.message!,
  //       status: 'error',
  //     };
  //   } else {
  //     console.error('Không tìm thấy kho:', result.message);
  //   }
  // }
  async handleSearchWarehouse() {
    const searchText = this.searchTextIn?.trim();

    if (!searchText) {
      // Hiển thị toàn bộ kho khi không nhập gì
      await this.getWarehouses();
      this.dataNotification = {
        messages: 'Đã hiển thị toàn bộ danh sách kho.',
        status: 'success',
      };
      return;
    }

    const result: BaseResponseModel = await this.warehouseEmployeeService.getWarehousesByName(searchText);

    if (result.isSuccess) {
      if (result.data && result.data.length > 0) {
        // Có dữ liệu trả về
        this.warehouses = result.data;
        this.dataNotification = {
          messages: result.message!,
          status: 'success',
        };
      } else {
        // Không có dữ liệu trả về
        this.warehouses = [];
        this.dataNotification = {
          messages: 'Không tìm thấy kho nào phù hợp với tên đã nhập.',
          status: 'info',
        };
      }
    } else {
      // Xử lý khi API trả về lỗi
      console.error('Lỗi khi tìm kiếm:', result.message);
      this.dataNotification = {
        messages: result.message!,
        status: 'error',
      };
    }
  }
}
