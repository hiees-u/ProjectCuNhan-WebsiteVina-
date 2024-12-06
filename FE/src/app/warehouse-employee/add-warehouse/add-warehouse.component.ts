// import { PostWareHouseRequestWarehouseEmployee } from './../warehouse-employee.module';
import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import { NotificationComponent } from "../../shared/item/notification/notification.component";
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import {
  PostWareHouseRequestWarehouseEmployee,
  ContructorPostWarehouseModule
} from '../warehouse-employee.module';
import { ProvinceComponent } from '../../shared/item/province/province.component';
import { DistrictComponent } from '../../shared/item/district/district.component';
import { CommuneComponent } from '../../shared/item/commune/commune.component';
import { AddressComponent } from '../../shared/item/address/address.component';
import {
  Address,
  AddressRequest,
  ConstructorAddress,
} from '../../shared/module/address/address.module';
import {
  CommuneResponseModel,
  ConstructorCommune,
} from '../../shared/module/commune/commune.module';
import { ServicesService } from '../../shared/services.service';
import { Router } from '@angular/router';
import {
  BaseResponseModel,
  BaseResponseModule,
} from '../../shared/module/base-response/base-response.module';
import { CustomerService } from '../../customer/customer.service';
@Component({
  selector: 'app-add-warehouse',
  imports: [
    CommonModule,
    FormsModule,
    ProvinceComponent,
    DistrictComponent,
    CommuneComponent,
    AddressComponent,
    NotificationComponent],
  templateUrl: './add-warehouse.component.html',
  styleUrl: './add-warehouse.component.css'
})
export class AddWarehouseComponent {

  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAdd: boolean = false;
  warehouse: PostWareHouseRequestWarehouseEmployee = ContructorPostWarehouseModule();

  //--
  addre: Address = ConstructorAddress();
  provinceSelect: number = 62;
  districtSelect: number = 1;
  communenInsert: CommuneResponseModel = {
    communeId: 1,
    communeName: '',
    districtId: 1,
  };

  //=============Notification
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(
    private warehouseEmployeeService: WarehouseEmployeeService,
    private servicee: ServicesService,
    private serviceCus: CustomerService,
    private router: Router
  ) { }

  checkWarehouseProperties() {
    this.isAdd = !!this.warehouse.warehouseName;
  }

  onDelete() {
    this.warehouse = ContructorPostWarehouseModule();
    this.checkWarehouseProperties();
  }

  async onPostWarehouse() {
    const addressRequest: AddressRequest = {
      communeId: this.communenInsert.communeId,
      communeName: this.communenInsert.communeName,
      districtId: this.districtSelect,
      houseNumber: this.addre.houseNumber,
      note: this.addre.note,
    };
    // gọi api post address trả về id address gán vào addressId
    const response: BaseResponseModel = await this.serviceCus.postAddress(
      addressRequest
    );

    if (response.isSuccess) {
      this.warehouse.addressId = response?.data;
      const response1: BaseResponseModel =
        await this.warehouseEmployeeService.postWarehouse(this.warehouse);
      if (response1.isSuccess) {
        this.dataNotification = {
          messages: response1.message!,
          status: 'success',
        };
      } else {
        this.dataNotification = {
          messages: 'Vui lòng thử lại',
          status: 'error',
        };
      }
    } else {
      this.dataNotification = {
        messages: 'Vui lòng thử lại',
        status: 'error',
      };
    }
    this.trigger = Date.now();
  }

  sendIsClose() {
    this.onDelete();
    this.isClose.emit(false);
  }

  ngOnInit(): void {
  }

  //nhận 1 addres
  getAddressChangeChildComponent(address: Address) {
    this.addre = address;
  }
  //nhận tỉnh id đang được selected từ component con
  getProvinceIDChangeChildComponent(provinceId: number) {
    this.provinceSelect = provinceId;
  }

  getDistrictIDChangeChildComponent(districtId: number) {
    this.districtSelect = districtId;
    this.communenInsert.districtId = districtId;
  }

  getCommutNameChangeChildComponent(commune: CommuneResponseModel) {
    this.communenInsert = commune;
  }
}
