import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { CommonModule } from '@angular/common';
import {
  Products, ContructorProductsResponseModule,
  InfoProducts, ContructorInfoProductsResponseModule
} from '../warehouse-employee.module';

import {
  BaseResponseModel, BaseResponseModule
} from '../../shared/module/base-response/base-response.module';


@Component({
  selector: 'app-product-warehouse',
  imports: [FormsModule, ReactiveFormsModule,
    CommonModule],
  templateUrl: './product-warehouse.component.html',
  styleUrl: './product-warehouse.component.css'
})
export class ProductWarehouseComponent {
  constructor(private warehouseEmployeeService: WarehouseEmployeeService) { }

  products: Products[] = [];
  inFoProducts: InfoProducts[] = [];
  selectedProduct: number = 0;

  async getProducts() {
    const data = await this.warehouseEmployeeService.getAllProducts();
    this.products = data.data;
  }
  async onProductChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    if (target) {
      const ProductId = Number(target.value);
      this.selectedProduct = ProductId;

      if (ProductId > 0) {
        try {
          const data = await this.warehouseEmployeeService.getInfoProductsByProductID(ProductId);
          if (data && data.data && Array.isArray(data.data)) {
            // Áp dụng hàm map với cấu trúc mặc định
            this.inFoProducts = data.data.map((product: Partial<InfoProducts>) => {
              const detail = ContructorInfoProductsResponseModule();
              return { ...detail, ...product }; // Kết hợp cấu trúc mặc định và dữ liệu API
            });
          } else {
            this.inFoProducts = [];
          }
        } catch (error) {
          console.error('Error fetching order details:', error);
          this.inFoProducts = [];
        }
      } else {
        this.inFoProducts = [];
      }
    }
  }


  ngOnInit(): void {
    this.getProducts();
  }
}
