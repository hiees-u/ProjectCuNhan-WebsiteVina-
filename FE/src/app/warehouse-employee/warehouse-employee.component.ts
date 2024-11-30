import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { WarehouseEmployeeService } from './warehouse-employee.service';
import { response } from 'express';

@Component({
  selector: 'app-warehouse-employee',
  standalone: true,
  imports: [
    RouterModule
  ],
  templateUrl: './warehouse-employee.component.html',
  styleUrl: './warehouse-employee.component.css'
})
export class WarehouseEmployeeComponent {
  constructor(private router: Router) {}
  
  click_add_warehouse() {
    // Điều hướng tới trang thêm kho
    // console.log('add-warehouse');
    this.router.navigate(['/warehouse-employee/add-warehouse']);
  }

  click_warehouse_management() {
    // Điều hướng tới trang quản lý kho
    this.router.navigate(['/warehouse-employee/warehouse-management']);
    // console.log('warehouse-management');
    // this.getWareHouse();    
  }

  // getWareHouse() {
  //   this.service.getWarehouses().subscribe(
  //     (response.isSuccess)
  //   )
  // }
}
