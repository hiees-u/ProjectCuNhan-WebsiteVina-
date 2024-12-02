import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { ContructorSupplierRequestModerator, SupplierRequestModerator } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { ViewProductsComponent } from '../view-products/view-products.component';
import { ModeratorService } from '../moderator.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";

@Component({
    selector: 'app-supplier-detail',
    imports: [
        FormsModule,
        ViewProductsComponent,
        NotificationComponent
    ],
    templateUrl: './supplier-detail.component.html',
    styleUrl: './supplier-detail.component.css'
})
export class SupplierDetailComponent {
  @Input() Supplier: SupplierRequestModerator = ContructorSupplierRequestModerator();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  isShowInsertAddress: boolean = false;

  addressString: string = ''
  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();
  
  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if(changes['Supplier']) {
      this.getAddressString();
    }
    console.log('Đang lấy địa chỉ string', this.addressString);
    
  }

  constructor(
    private moderatorService: ModeratorService
  ) {}

  async getAddressString() {
    const result : BaseResponseModel = await this.moderatorService.getStringAddresses(this.Supplier.addressId);
    if(result.isSuccess){
      this.addressString = result.data[0].value;
    }
  }

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }

  async handleUpdate() {
    console.log(this.Supplier);
    const result: BaseResponseModel = await this.moderatorService.putSupplier(this.Supplier);
    if(result.isSuccess) {
      this.dataNotification.status = 'success';
    } else {
      this.dataNotification.status = 'error';
    }
    this.dataNotification.messages = result.message!;
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây 
    setTimeout(() => { this.trigger = undefined; }, 30000);
  }

  async handleDelete() {
    console.log(this.Supplier);
    const result: BaseResponseModel = await this.moderatorService.deleteSupplier(this.Supplier.supplierId);
    if(result.isSuccess) {
      this.dataNotification.status = 'success';
    } else {
      this.dataNotification.status = 'error';
    }
    this.dataNotification.messages = result.message!;
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây 
    setTimeout(() => { this.trigger = undefined; }, 30000);
  }
}
