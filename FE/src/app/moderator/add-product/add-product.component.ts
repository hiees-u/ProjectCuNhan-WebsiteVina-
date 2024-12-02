import { Component, EventEmitter, Output } from '@angular/core';
import { ContructorInsertProduct, InsertProduct } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Category } from '../../shared/module/category/category.module';
import { ServicesService } from '../../shared/services.service';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { Supplier } from '../../shared/module/supplier/supplier.module';
import { ModeratorService } from '../moderator.service';
import { AddCateComponent } from "../add-cate/add-cate.component";
import { AddSubCateComponent } from "../add-sub-cate/add-sub-cate.component";
import { AddSupplierComponent } from "../add-supplier/add-supplier.component";

@Component({
    selector: 'app-add-product',
    imports: [FormsModule, CommonModule, AddCateComponent, AddSubCateComponent, AddSupplierComponent],
    templateUrl: './add-product.component.html',
    styleUrl: './add-product.component.css'
})
export class AddProductComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private service: ServicesService, private moderatorService: ModeratorService ) {}

  ngOnInit(): void {    
    this.getCategorys();
    this.getSubCatgorys();
    this.getSupplier();
    this.onProductChange();
  }

  product: InsertProduct = ContructorInsertProduct();
  isAdd: boolean = false;

  categorys: Category[] = [];
  subCategorys: SubCategory[] = [];
  supplier: Supplier[] = [];

  selectedFile: File | null = null;

  isShow: boolean = false;
  flag:boolean = true;

  isShowBy: number = 1;

  isShowContenSecond(i: number) {
    this.isShow = !this.isShow;
    this.isShowBy = i;
    console.log(this.isShowBy);
  }

  handleClose(is: boolean) {
    this.isShow = !is;
    this.flag = true;
    this.getCategorys();
    this.getSubCatgorys();
    this.getSupplier();
    this.onProductChange();
  }

  async getCategorys() {
    const data = await this.service.GetAllCate();
    this.categorys = data.data.data;
    this.product.categoryId = this.categorys[0].categoryId;
  }

  async getSubCatgorys() {
    const data = await this.service.GetAllSubCate();
    this.subCategorys = data.data.data;
    this.product.subCategoryId = this.subCategorys[0].subCategoryId;
  }

  async getSupplier() {
    const data = await this.service.GetAllSupplier();
    this.supplier = data.data.data;
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

  onProductImageChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if(input && input.files && input.files[0]) {
      this.selectedFile = input.files[0];   
    }
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
    console.log('tới đây', new Date());
    
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

  async onUpdate() {
    await this.uploadFile();
    if(this.isAdd) {
      const response = await this.moderatorService.postProduct(this.product);

      //check response
      if(response.isSuccess) {
        console.log('Thêm sản phẩm thành công...');
        this.sendIsClose();
      } else {
        console.log('Quá trình thêm sản phẩm bị gián đoạn!..');        
      }
    } else {
      console.log('vui lòng kiểm tra lại thông tin...');
    }
    console.log(this.product);
  }

  async uploadFile() {
    if(this.selectedFile) {
      try {        
        const response = await this.moderatorService.UploadFile(this.selectedFile);
        this.product.image = response.data; //gán tên ảnh vào product để request api
        console.log('file uploaded successfully!', response.isSuccess);
        //gọi api post product với this.product
      } catch (error) {
        console.log('File upload failed!!');
      }
    } 
  }
}
