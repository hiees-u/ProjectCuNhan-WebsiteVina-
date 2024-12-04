import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  WareHouseRequestWarehouseEmployee,
  ContructorRequestWarehouseModule
} from '../warehouse-employee.module';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
@Component({
  selector: 'app-view-warehouse',
  imports: [
    FormsModule,
    CommonModule,
    NotificationComponent],
  templateUrl: './view-warehouse.component.html',
  styleUrl: './view-warehouse.component.css'
})
export class ViewWarehouseComponent {
  @Input() Warehouse: WareHouseRequestWarehouseEmployee = ContructorRequestWarehouseModule();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  isShowInsertAddress: boolean = false;

  addressString: string = '';

  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['Warehouse']) {
      this.getAddressString();
    }
  }
  constructor(
    private service: WarehouseEmployeeService,
  ) { }

  async getAddressString() {
    const result: BaseResponseModel = await this.service.getStringAddresses(this.Warehouse.addressId);
    if (result.isSuccess) {
      this.addressString = result.data[0].value;
    }
  }

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }

  async handleUpdate() {
    const result: BaseResponseModel = await this.service.putWarehouse(this.Warehouse);
    if (result.isSuccess) {
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
    const result: BaseResponseModel = await this.service.deleteWarehouse(this.Warehouse.warehouseId);
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
