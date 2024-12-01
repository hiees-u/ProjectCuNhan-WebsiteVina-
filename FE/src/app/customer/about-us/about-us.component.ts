import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from '../../shared/module/product/product.module';
import { ProductItemComponent } from "../../shared/item/product-item/product-item.component";
import { CommonModule } from '@angular/common';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'app-about-us',
  standalone: true,
  imports: [
    ProductItemComponent,
    CommonModule
  ],
  templateUrl: './about-us.component.html',
  styleUrl: './about-us.component.css'
})
export class AboutUsComponent {
  products: Product[] = [];

  constructor(private router: Router, private customer_service: CustomerService) {}

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.getProduct();
  }

  navigateToViewProduct() {
    this.router.navigate(['/customer/view-product']);
  }

  async getProduct(
    productId: number | null = null,
    cateId: number | null = null,
    subCateId: number | null = null,
    supplierId: number | null = null,
    productName: string | null = null,
    pageNumber: number | 1 = 1,
    pageSize: number | 1 = 3,
    sortByName: number | 0 = 1,
    sortByPrice: number | 0 = 0
  ) {
    try {
      const response = await this.customer_service.getProducts(
        productId,
        cateId,
        subCateId,
        supplierId,
        productName,
        pageNumber,
        pageSize,
        sortByName,
        sortByPrice
      );
      if (response.isSuccess) {
        this.products = response.data.products;
        console.log(`CHẠY LẠI GET PRODUCT ${new Date()}`);
      } else {
        console.log('Failed to get products');
      }
    } catch (error) {
      console.error('Error:', error);
      console.log('An error occurred while fetching the products.');
    }
  }

  async handleDataChange(data: Product) {
    // this.show = 1;
    // this.receivedData = data;
    // this.cart.quantity = 1;
    // this.cart.productId = data.productId;
    // await this.getSubCateProductDetail(this.receivedData?.productId);
    // await this.getCateProductDetail(this.receivedData?.productId);
    // await this.getSupplier(this.receivedData?.supplier!);
    // console.log(this.cart);
    // console.log(this.receivedData);
    console.log('handleDataChange', data);   
  }

}
