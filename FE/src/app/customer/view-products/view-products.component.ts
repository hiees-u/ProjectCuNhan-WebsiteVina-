import { Component } from '@angular/core';
import { NgIf, SlicePipe } from '@angular/common';
import { NgFor } from '@angular/common';
import { NgClass } from '@angular/common';
import { CommonModule } from '@angular/common';

import { CustomCurrencyPipe } from '../../shared/module/customCurrency';

import { ProductItemComponent } from '../../shared/item/product-item/product-item.component';
// import { FilterPriceComponent } from '../../shared/item/filter-price/filter-price.component';
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
    imports: [
        CommonModule,
        NgIf,
        NgFor,
        NgClass,
        ProductItemComponent,
        SearchBoxComponent,
        CustomCurrencyPipe,
        NotificationComponent,
    ],
    templateUrl: './view-products.component.html',
    styleUrls: ['./view-products.component.css', './product-detail.css']
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
  pages: number[] = [];
  pageShow: number[] = [];
  //--
  logError: string = '';
  //-- Đang lấy dữ liệu theo search?
  searchString: string | undefined;

  //SORT
  sortByName: number = 0;
  sortByPrice: number = 0;

  constructor(private customer_service: CustomerService) {}

  //Sort Handle
  handleSort(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const value = selectElement.value;
    console.log(`Value đang được select: ${value}`);

    switch (parseInt(value, 10)) {
      case 1:
        this.sortByName = -1;
        this.sortByPrice = 0;
        break;
      case 2:
        this.sortByName = 1;
        this.sortByPrice = 0;
        break;
      case 3:
        this.sortByName = 0;
        this.sortByPrice = -1;
        break;
      case 4:
        this.sortByName = 0;
        this.sortByPrice = 1;
        break;

      default:
        this.sortByName = 0;
        this.sortByPrice = 0;
        break;
    }
    this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      this.searchString,
      this.pageActive,
      8,
      this.sortByName,
      this.sortByPrice
    );
  }

  //nhận dữ liệu từ Search Box
  async receiveData(data: string) {
    this.searchString = data;
    this.pageActive = 1;
    console.log(`Nhận lại ${this.searchString}`);
    await this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      this.searchString,
      this.pageActive,
      8,
      this.sortByName,
      this.sortByPrice
    );
  }

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
        console.log('Bắn ra lỗi');
        this.dataNotification.status = 'error';
        this.dataNotification.messages = 'Vui lòng đăng nhập';
        this.trigger = new Date();
        this.responseMessage.message =
          'An error occurred while adding the product to cart.';
      }
    }
  }

  handleCateActive(CateId: number) {
    this.cateActive = CateId === this.cateActive ? null : CateId;
    this.pageActive = 1;
    this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      this.searchString,
      this.pageActive,
      8,
      this.sortByName,
      this.sortByPrice
    );
  }

  handlePageClick(page: number) {
    this.pageActive = page;
    console.log(`CLICK VÀO PAGE NUMBER ${page}`, new Date());
    //lấy dữ liệu như bình thường
    this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      this.searchString,
      this.pageActive,
      // 2,
      8,
      this.sortByName,
      this.sortByPrice
    );
  }

  handleSubCateActive(CateId: number) {
    this.subCateActive = CateId === this.subCateActive ? null : CateId;
    this.pageActive = 1;
    this.getProduct(
      null,
      this.cateActive,
      this.subCateActive,
      null,
      this.searchString,
      this.pageActive,
      8,
      this.sortByName,
      this.sortByPrice
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
        this.products = response.data.products;
        this.pages = [];
        for (let index = 0; index < response.data.totalPages; index++) {
          this.pages.push(index + 1);
        }
        // Giả sử `totalPages` là tổng số trang hiện có
        let totalPages = this.pages.length;

        // Nếu lần đầu (pageActive là 1), start ở đầu
        let start = 0;
        if (this.pageActive > 1) {
          // Những lần sau, start sẽ nằm ở giữa để trang hiện tại ở giữa
          start = this.pageActive - 2;
          start = Math.max(start, 0); // Đảm bảo không nhỏ hơn 0
        }

        // Thiết lập end sao cho luôn lấy 3 phần tử
        let end = start + 3;
        end = Math.min(end, totalPages); // Đảm bảo không vượt quá tổng số trang

        // Điều chỉnh lại start nếu end - start < 3
        if (end - start < 3) {
          start = end - 3;
          start = Math.max(start, 0); // Đảm bảo không nhỏ hơn 0
        }

        this.pageShow = this.pages.slice(start, end);

        // console.log(response.data.totalPages);
        console.log(`CHẠY LẠI GET PRODUCT ${new Date()}`);
        console.log(this.products);
        if(this.products.length === 0) {
          console.log('không tìm thấy sản phẩm...!');
          this.logError = 'Không tìm thấy sản phẩm...!'
          
        }
      } else {
        console.log('Failed to get products');
      }
    } catch (error) {
      console.error('Error:', error);
      console.log('An error occurred while fetching the products.');
    }
  }

  async ngOnInit(): Promise<void> {
    await this.getTop10Categories();
    await this.getTop10SubCategories();
    await this.getProduct();

    this.dataNotification.status = 'info';
    this.dataNotification.messages = '';

    if (this.products.length === 0) {
      this.logError = 'Không có sản phẩm nào...';
    }
  }

  async handleDataChange(data: Product) {
    this.show = 1;
    this.receivedData = data;
    this.cart.quantity = 1;
    this.cart.productId = data.productId;
    await this.getSubCateProductDetail(this.receivedData?.productId);
    await this.getCateProductDetail(this.receivedData?.productId);
    await this.getSupplier(this.receivedData?.supplier!);
    console.log(this.cart);
    console.log(this.receivedData);
    console.log('handleDataChange', data);   
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
        console.log(new Date() , productID);

        this.cateProductDetail = response.data;
        console.log(new Date() + this.cateProductDetail);
        
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
