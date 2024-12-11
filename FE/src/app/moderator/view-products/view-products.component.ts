import { Component, Input, SimpleChanges } from '@angular/core';
import {
  ContructorProductModerator,
  ProductModerator,
} from '../moderator.module';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ModeratorService } from '../moderator.service';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import { ProductDetailComponent } from '../product-detail/product-detail.component';

@Component({
  selector: 'app-view-products',
  imports: [
    CommonModule,
    CustomCurrencyPipe,
    FormsModule,
    NotificationComponent,
    ProductDetailComponent,
  ],
  templateUrl: './view-products.component.html',
  styleUrl: './view-products.component.css',
})
export class ViewProductsComponent {
  constructor(
    private service: ModeratorService,
    private sanitizer: DomSanitizer
  ) {}

  @Input() key: number = 0;

  @Input() cateId: number | null = null;
  @Input() subCateId: number | null = null;
  @Input() supplierId: number | null = null;

  toDay: Date = new Date();

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  @Input() searchText: string = '';
  isShowAddProduct: boolean = false;
  flag: boolean = false;

  products: ProductModerator[] = [];

  isError: boolean = false;

  imageUrls: { [key: number]: SafeUrl } = {};
  defaultImageUrl: string = '/assets/Products/p1.png';

  flagDetail: boolean = false;
  isShowDetail: boolean = false;

  isShowDelete: boolean = false;
  flagDelete: boolean = false;

  selectProduct: ProductModerator = ContructorProductModerator();

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  ngOnInit(): void {
    this.flag = false;
    this.flagDelete = false;
    this.flagDetail = false;
    this.getProduct();

    this.products.forEach((item, index) => {
      this.checkImageExistence(`/assets/Products/${item.image}`, index);
    });
  }

  isExpired(expriryDate: string): boolean {
    //true => hết hạn
    const _expriryDate = new Date(expriryDate);
    return _expriryDate < this.toDay;
  }

  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if (changes['searchText']) {
      this.getProduct();
    }
    if (changes['cateId']) {
      console.log('Tới đây rồi: ', this.cateId);

      this.getProduct();
    }
    if (changes['subCateId']) {
      console.log('Tới đây rồi: ', this.subCateId);

      this.getProduct();
    }
    if (changes['supplierId']) {
      console.log('Tới đây rồi: ', this.subCateId);

      this.getProduct();
    }
    if (changes['key']) {
      this.getProduct();
    }
  }

  checkImageExistence(url: string, index: number) {
    fetch(url, { method: 'HEAD' }).then((response) => {
      if (response.ok) {
        this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(url);
      } else {
        this.handleImageErrors(index, this.products[index].image);
      }
    });
  }

  onChangePage(page: number) {
    this.pageCurrent = page;
    this.getProduct();
  }

  onChangeSearch(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value;
  }

  getProduct(): void {
    console.log('cập nhật product');

    this.service
      .getProducts(
        null,
        this.cateId,
        this.subCateId,
        this.supplierId,
        this.searchText,
        this.pageCurrent,
        7,
        0,
        0
      )
      .then((data) => {
        this.products = data.data.products;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage)
          .fill(0)
          .map((x, i) => i + 1);
        console.log(this.products);
        if (this.products.length <= 0) this.isError = true;
        else this.isError = false;
      })
      .catch((error) => {
        console.error('Error fetching product', error);
      });
  }

  handlePageClick(page: number) {
    if (page === 1) {
      if (this.pageCurrent !== this.totalPage) this.pageCurrent += 1;
    } else {
      if (this.pageCurrent !== 1) this.pageCurrent -= 1;
    }
    this.getProduct();
  }

  async handleImageErrors(index: number, fileName: string): Promise<void> {
    const imageUrl = await this.service.getImage(fileName);
    if (imageUrl) {
      this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(imageUrl);
    } else {
      this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(
        this.defaultImageUrl
      );
    }
    console.log('Không tìm thấy ảnh??', this.imageUrls[index]);
  }

  handleClose(is: boolean) {
    console.log('THOÁT THÊM');
    this.isShowDetail = !is;
    this.getProduct();
    this.flag = true;
  }

  onShowDelete(product: ProductModerator) {
    this.selectProduct = product;
    this.isShowDelete = true;
    this.flagDelete = true;
    console.log(this.selectProduct);
  }

  async deleteProduct() {
    try {
      const result = await this.service.deleteProduct(
        this.selectProduct.productId
      );
      if (result.isSuccess) {
        this.getProduct();
        this.dataNotification.status = 'success';
      } else {
        this.dataNotification.status = 'error';
      }
      this.dataNotification.messages = result.message!;
      this.trigger = Date.now();
    } catch (error) {
      console.log(error);
    }
    setTimeout(() => {
      this.onHidenDelete();
    }, 500);
  }

  onHidenDelete() {
    this.isShowDelete = !this.isShowDelete;
    setTimeout(() => {
      this.selectProduct = ContructorProductModerator();
    }, 400);
  }

  onShowDetail(product: ProductModerator) {
    this.selectProduct = product;
    this.isShowDetail = true;
    this.flagDetail = true;
    console.log(this.selectProduct);
  }

  sendCloseDetail(is: boolean) {
    this.isShowDetail = false;
    this.selectProduct = ContructorProductModerator();
  }
}
