import { Component } from '@angular/core';
import { OrderProductsComponent } from '../order-products/order-products.component';
import { NgIf } from '@angular/common';
import { NgFor } from '@angular/common';
import { NgClass } from '@angular/common';
import { CommonModule } from '@angular/common';

import { CustomCurrencyPipe } from '../../shared/module/customCurrency';

import { ProductItemComponent } from '../../shared/item/product-item/product-item.component';
import { FilterPriceComponent } from '../../shared/item/filter-price/filter-price.component';
import { CustomerService } from '../customer.service';
import { Product } from '../../shared/module/product/product.module';
import {
  CartResponse,
  constructorCartResponse,
} from '../../shared/module/cart/cart.module';
import { SearchBoxComponent } from '../../shared/item/search-box/search-box.component';
import { Category } from '../../shared/module/category/category.module';
import {
  BaseResponseModel,
  constructorBaseResponseModule,
} from '../../shared/module/base-response/base-response.module';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { Supplier } from '../../shared/module/supplier/supplier.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';

@Component({
  selector: 'app-view-products',
  standalone: true,
  imports: [
    // OrderProductsComponent,
    CommonModule,
    NgIf,
    NgFor,
    NgClass,
    FilterPriceComponent,
    ProductItemComponent,
    SearchBoxComponent,
    CustomCurrencyPipe,
    NotificationComponent,
  ],
  templateUrl: './view-products.component.html',
  styleUrls: ['./view-products.component.css', './product-detail.css'],
})
export class ViewProductsComponent {
  
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  show: number = 0;
  receivedData: Product | undefined;
  products: Product[] = [];
  responseMessage: BaseResponseModel = constructorBaseResponseModule();
  cate: Category[] = [];
  subCate: SubCategory[] = [];
  cart: CartResponse = constructorCartResponse();
  cateProductDetail: string = '';
  subCateProductDetail: string = '';
  supplierProductDetail: Supplier | undefined;

  //--
  cateActive: any = null;
  subCateActive: any = null;
  //--
  pageActive: number = 1;
  pages = [1, 2, 3];
  //--
  logError: string = '';

  constructor(private customer_service: CustomerService) {}

  async handleAddCart() {
    try {
      const response = await this.customer_service.postCart(this.cart);
      if (response.isSuccess) {
        console.log('Product added to cart successfully');
        this.dataNotification.status = 'success';
        this.dataNotification.messages =
          'Thêm sản phẩm vào giỏ hàng thành công!!';
        this.trigger = Date.now();
      } else {
        if (this.responseMessage) {
          this.responseMessage.isSuccess = false;
          this.responseMessage.message =
            'Có lỗi trong quá trình Thêm sản phẩm vào giỏ hàng. Vui lòng thử lại!!';
        }
      }
    } catch (error) {
      console.error('Error:', error);
      if (this.responseMessage) {
        this.responseMessage.isSuccess = false;
        this.responseMessage.message =
          'An error occurred while adding the product to cart.';
      }
    }
  }

  handleCateActive(CateId: number) {
    this.cateActive = CateId === this.cateActive ? null : CateId;
    this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      null,
      this.pageActive,
      8,
      0,
      0
    );
  }

  handlePageClick(page: number) {
    this.pageActive = page;
    this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      null,
      this.pageActive,
      8,
      0,
      0
    );
  }

  handleSubCateActive(CateId: number) {
    this.subCateActive = CateId === this.subCateActive ? null : CateId;
    this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      null,
      this.pageActive,
      8,
      0,
      0
    );
  }

  //--lấy 10 subcate
  async getTop10SubCategories() {
    try {
      const response: BaseResponseModel =
        await this.customer_service.getTop10SubCate();
      if (response.isSuccess) {
        this.subCate = response.data;
      } else {
        console.error('Failed to get categories');
      }
    } catch (error) {
      console.error('Error:', error);
    }
  }

  //--lấy 10 cate
  async getTop10Categories() {
    try {
      const response: BaseResponseModel =
        await this.customer_service.getTop10Cate();
      if (response.isSuccess) {
        this.cate = response.data;
      } else {
        console.error('Failed to get categories');
      }
    } catch (error) {
      console.error('Error:', error);
    }
  }

  async getSupplier(supplierId: number) {
    try {
      const response: BaseResponseModel =
        await this.customer_service.getSupplierByID(supplierId);
      if (response.isSuccess) {
        this.supplierProductDetail = response.data;
      } else {
        console.log('Failed to get categories');
      }
    } catch (error) {
      console.error('Error:', error);
      console.log('An error occurred while fetching the categories');
    }
  }

  async getProduct(
    productId: number | null = null,
    cateId: number | null = null,
    subCateId: number | null = null,
    supplierId: number | null = null,
    productName: string | null = null,
    pageNumber: number | 1 = 1,
    pageSize: number | 1 = 8,
    sortByName: number | 0 = 0,
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
        this.products = response.data;
        console.log(this.products);
        
      } else {
        console.log('Failed to get products');
      }
    } catch (error) {
      console.error('Error:', error);
      console.log('An error occurred while fetching the products.');
    }
  }

  async ngOnInit(): Promise<void> {
    this.getTop10Categories();
    this.getTop10SubCategories();
    this.getProduct();

    this.dataNotification.status = 'info';
    this.dataNotification.messages = '';

    if (this.products.length === 0) {
      this.logError = 'Không có sản phẩm nào...';
    }
  }

  handleDataChange(data: Product) {
    this.show = 1;
    this.receivedData = data;
    this.cart.quantity = 1;
    this.cart.productId = data.productId;
    this.getSubCateProductDetail(this.receivedData?.subCategoryId!);
    this.getCateProductDetail(this.receivedData?.categoryId!);
    this.getSupplier(this.receivedData?.supplier!);
    console.log(this.cart);
    console.log(this.receivedData);
  }

  async getSubCateProductDetail(productID: number) {
    try {
      const response: BaseResponseModel =
        await this.customer_service.getSubCateByProductID(productID);
      if (response.isSuccess) {
        this.subCateProductDetail = response.data;
      } else {
        console.log('Failed to get categories');
      }
    } catch (error) {
      console.error('Error:', error);
      console.log('An error occurred while fetching the categories');
    }
  }

  async getCateProductDetail(productID: number) {
    try {
      const response: BaseResponseModel =
        await this.customer_service.getCateByProductID(productID);
      if (response.isSuccess) {
        this.cateProductDetail = response.data;
      } else {
        console.log('Failed to get categories');
      }
    } catch (error) {
      console.error('Error:', error);
      console.log('An error occurred while fetching the categories');
    }
  }

  handleClose() {
    this.show = -1;
    // this.receivedData = undefined;
  }

  InDeCrease(InDe: number) {
    if (this.cart) {
      if (InDe === 1) {
        console.log(this.cart);
        if (this.receivedData!.totalQuantity > this.cart.quantity) {
          console.log('đây');
          this.cart.quantity += InDe;
        }
      } else {
        this.cart.quantity += InDe;
      }

      // Đảm bảo số lượng không bao giờ nhỏ hơn 0
      if (this.cart.quantity < 1) {
        this.cart.quantity = 1;
      }
    }
  }
}
