import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  ContructorProductModerator,
  ProductModerator,
} from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { ServicesService } from '../../shared/services.service';
import { ModeratorService } from '../moderator.service';
import { Category } from '../../shared/module/category/category.module';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { Supplier } from '../../shared/module/supplier/supplier.module';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [FormsModule, CommonModule, NotificationComponent],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css',
})
export class ProductDetailComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() product: ProductModerator = ContructorProductModerator();

  productName: string = '';

  categorys: Category[] = [];
  subCategorys: SubCategory[] = [];
  supplier: Supplier[] = [];

  imageUrls: { [key: number]: SafeUrl } = {};

  selectedFile: File | null = null;

  imgName = '';

  expiryDate: string = '';

  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(
    private service: ServicesService,
    private moderatorService: ModeratorService
  ) {}

  async onUpdate() {
    this.product.image = this.imgName;
    console.log(this.product);
    const result: BaseResponseModel = await this.moderatorService.putProduct(
      this.product
    );

    if (result.isSuccess) {
      this.dataNotification.status = 'success';
      this.dataNotification.messages = result.message!;

      setTimeout(() => {
        this.trigger = undefined;
        this.sendIsClose();
      }, 4000);
    } else {
      this.dataNotification.messages = 'Vui lòng kiểm tra lại..!';
      this.dataNotification.status = 'error';
    }
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây
    setTimeout(() => {
      this.trigger = undefined;
    }, 4000);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['product']) {
      this.productName = this.product.productName;
      // this.getCategorys();
      this.imgName = this.product.image;
      this.getCategorys();
      this.getSubCatgorys();
      this.getSupplier();
    }
  }

  async getCategorys() {
    const data = await this.service.GetAllCate();
    this.categorys = data.data.data;
    console.log(this.categorys);

    // this.product.categoryId = this.categorys[0].categoryId;
  }

  async getSubCatgorys() {
    const data = await this.service.GetAllSubCate();
    this.subCategorys = data.data.data;
    console.log(this.subCategorys);
    // this.product.subCategoryId = this.subCategorys[0].subCategoryId;
  }

  async getSupplier() {
    const data = await this.service.GetAllSupplier();
    this.supplier = data.data.data;
    console.log(this.supplier);

    // this.product.supplier = this.supplier[0].supplierId;
  }

  onProductImageChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.imgName = file.name;
    }
  }

  ngOnInit(): void {
    this.getCategorys();
    this.getSubCatgorys();
    this.getSupplier();
    // this.product.expriryDate = this.formatDateForInput(this.product.expriryDate);
    if (this.product.expriryDate) {
      this.product.expriryDate = this.formatDateForInput(
        this.product.expriryDate
      );
    }
  }

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }

  get formattedPrice(): string {
    return this.formatCurrency(this.product.price);
  }
  set formattedPrice(value: string) {
    this.product.price = this.parseCurrency(value);
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND',
    }).format(value);
  }
  parseCurrency(value: string): number {
    // Loại bỏ các ký tự không phải là số hoặc dấu tách thập phân
    const cleanedValue = value.replace(/[^0-9,-]+/g, '').replace(',', '.');
    return parseFloat(cleanedValue);
  }

  //date
  get formattedExpiryDate(): string {
    return this.formatDateForInput(this.product.expriryDate);
  }
  set formattedExpiryDate(value: string) {
    this.product.expriryDate = this.formatDateForBackend(value);
  }

  formatDateForBackend(dateString: string): string {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
      return ''; // Trả về chuỗi rỗng nếu giá trị không hợp lệ
    }
    return date.toISOString();
  }

  formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
      return ''; // Trả về chuỗi rỗng nếu giá trị không hợp lệ
    }
    return date.toISOString().split('T')[0];
  }
}
