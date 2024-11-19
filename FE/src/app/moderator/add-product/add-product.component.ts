import { Component, EventEmitter, Output } from '@angular/core';
import { ContructorInsertProduct, InsertProduct } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Category } from '../../shared/module/category/category.module';
import { ServicesService } from '../../shared/services.service';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { Supplier } from '../../shared/module/supplier/supplier.module';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css',
})
export class AddProductComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private service: ServicesService) {}

  ngOnInit(): void {
    this.getCategorys();
    this.getCatgorys();
    this.getSupplier();
  }

  product: InsertProduct = ContructorInsertProduct();
  isAdd: boolean = false;

  categorys: Category[] = [];
  subCategorys: SubCategory[] = [];
  supplier: Supplier[] = [];

  async getCategorys() {
    const data = await this.service.GetAllCate();
    this.categorys = data.data.data;
    this.product.categoryId = this.categorys[0].categoryId;
  }

  async getCatgorys() {
    const data = await this.service.GetAllSubCate();
    this.subCategorys = data.data.data;
    this.product.subCategoryId = this.subCategorys[0].subCategoryId;
  }

  async getSupplier() {
    const data = await this.service.GetAllSupplier();
    this.supplier = data.data;
    this.product.supplier = this.supplier[0].supplierId;
  }

  checkProductProperties(): void {
    this.isAdd = !!(
      this.product.productName &&
      this.product.image &&
      this.product.categoryId !== null &&
      this.product.supplier !== null &&
      this.product.subCategoryId !== null &&
      this.product.expiryDate &&
      this.product.description &&
      this.product.price !== null
    );
  }

  onProductChange(): void {
    this.checkProductProperties();
    if (typeof this.product.expiryDate === 'string') {
      const dateValue = new Date(this.product.expiryDate);
      this.product.expiryDate = this.formatDateToLocal(dateValue);
    }
  }

  formatDateToLocal(date: Date): string {
    const tzoffset = date.getTimezoneOffset() * 60000;
    const localISOTime = new Date(date.getTime() - tzoffset).toISOString().slice(0, 16);
    return localISOTime;
  }

  sendIsClose() {
    this.onDelete();
    this.isClose.emit(true);
  }

  onDelete() {
    this.product = ContructorInsertProduct();
    this.checkProductProperties();
    this.isAdd = false;
    this.product.subCategoryId = this.subCategorys[0].subCategoryId;
    this.product.categoryId = this.categorys[0].categoryId;
    this.product.supplier = this.supplier[0].supplierId;
  }

  onUpdate() {
    console.log(this.product);
  }
}
