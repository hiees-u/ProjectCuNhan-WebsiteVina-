import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import { Warehouse } from '../warehouse-employee.module';
@Component({
  selector: 'app-warehouse-management',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './warehouse-management.component.html',
  styleUrls: ['./warehouse-management.component.css'],
  
})
export class WarehouseManagementComponent{
  warehouses: Warehouse[] = [];

  ngOnInit(): void {
    // this.getWarehouses();
  }
  constructor( private routes: Router, private warehouseEmployeeService: WarehouseEmployeeService) {}

  // async getWarehouses() {
  //   console.log('Vào Warehouse');
  //   const result: BaseResponseModel = await this.warehouseEmployeeService.getWarehouses();

  //   if(result.isSuccess) {
  //     this.warehouses = result.data;
  //   }
  // }
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
  
}
