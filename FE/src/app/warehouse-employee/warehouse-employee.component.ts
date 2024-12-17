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
      console.info = result.data;
    }
  }

  click_warehouse_managerment() {
    this.router.navigate(['/warehouse-employee/warehouse-management']);
  }

  click_out_warehouse() {
    this.router.navigate(['/warehouse-employee/out-warehouse']);
  }
  click_in_warehouse() {
    this.router.navigate(['/warehouse-employee/in-warehouse']);
  }
  click_products_expriry() {
    this.router.navigate(['/warehouse-employee/products-expriry']);
  }
  click_product_warehouse() {
    this.router.navigate(['/warehouse-employee/product-warehouse']);
  }
  logOutHandler() {
    this.router.navigate(['/login']);
    localStorage.removeItem('token');
  }
}
