import { Warehouse } from './warehouse-employee.module';
import { WarehouseEmployeeService } from './warehouse-employee.service';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { BaseResponseModel } from '../shared/module/base-response/base-response.module';

@Component({
    selector: 'app-warehouse-employee',
    imports: [
        RouterOutlet, CommonModule
    ],
    templateUrl: './warehouse-employee.component.html',
    styleUrl: './warehouse-employee.component.css'
})
export class WarehouseEmployeeComponent {

  ngOnInit(): void {
    // this.navigateToProduct();
    this.getWarehouses();
  }
  constructor(private router: Router, private WarehouseEmployeeeService: WarehouseEmployeeService) { }
  
  async getWarehouses() {
    const result: BaseResponseModel = await this.WarehouseEmployeeeService.getWarehouses();

    if(result.isSuccess) {
      console.log = result.data;
    }
  }

  click_warehouse_managerment() {
    this.router.navigate(['/warehouse-employee/warehouse-management']);
  }

  click_add_Test() {
    this.router.navigate(['/warehouse-employee/add-warehouse']);
  }
  logOutHandler() {
    this.router.navigate(['/login']);
    localStorage.removeItem('token');
  }
}
