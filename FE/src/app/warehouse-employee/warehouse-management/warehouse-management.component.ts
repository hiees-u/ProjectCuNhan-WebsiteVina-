import { AddWarehouseComponent } from './../add-warehouse/add-warehouse.component';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { BaseResponseModel, BaseResponseModule } from '../../shared/module/base-response/base-response.module';
import { Warehouse, ContructorWarehouse } from '../warehouse-employee.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-warehouse-management',
  // standalone: true,
  imports: [CommonModule,
    AddWarehouseComponent,
    RouterOutlet,
    FormsModule,
    NotificationComponent
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

  ngOnInit(): void {
    this.getWarehouses();
  }

  constructor(private router: Router, private warehouseEmployeeService: WarehouseEmployeeService) {
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

  getWarehouse() {
    this.warehouseEmployeeService
      .getSupplier(this.searchText)
      .then((data) => {
        this.Suppliers = data.data.data;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage)
          .fill(0)
          .map((x, i) => i + 1);
        console.log(this.Suppliers);
        console.log(this.pages);
        console.log(this.totalPage);
      })
      .catch((error) => {
        console.error('Error fetching product', error);
      });
  }
  handleCloseWD(is: boolean) {
    console.log('THOÁT THÊM');
    // this.getCategorys();
    this.isShowAddSupplier = !is;
    this.isShowDetail = !is;
    this.flag = false;
    this.getSupplier();
  }
}
