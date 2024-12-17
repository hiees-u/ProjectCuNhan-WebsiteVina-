import { Component } from '@angular/core';
import {
  ProductsExpriryDate, ContructorProductsExpriryDateResponseModule
} from '../warehouse-employee.module';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { CommonModule } from '@angular/common';
import {
  BaseResponseModel, BaseResponseModule
} from '../../shared/module/base-response/base-response.module';
import { Router } from '@angular/router';
@Component({
  selector: 'app-products-expriry',
  imports: [CommonModule],
  templateUrl: './products-expriry.component.html',
  styleUrl: './products-expriry.component.css'
})
export class ProductsExpriryComponent {


  constructor(private router: Router, private warehouseEmployeeService: WarehouseEmployeeService) {
  }

  productsExpriry: ProductsExpriryDate[] = [];
  ngOnInit(): void {
    this.getProductsExpriryDate();
  }
  async getProductsExpriryDate() {
    try {
      const result: BaseResponseModel = await this.warehouseEmployeeService.getProductsExpriryDate();
      if (result.isSuccess) {
        this.productsExpriry = result.data;
      } else {
        console.error('Không thể lấy dữ liệu ProductsExpriryDate:', result.message);
      }
    } catch (error) {
      console.error('Lỗi khi lấy dữ liệu ProductsExpriryDate:', error);
    }
  }
}
