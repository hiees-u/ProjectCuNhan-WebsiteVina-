import { Component, EventEmitter, Output } from '@angular/core';
import {
  ContructorSupplierRequestModerator,
  SupplierRequestModerator,
} from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AddressComponent } from '../../shared/item/address/address.component';
import { CommuneComponent } from '../../shared/item/commune/commune.component';
import { DistrictComponent } from '../../shared/item/district/district.component';
import { ProvinceComponent } from '../../shared/item/province/province.component';
import {
  Address,
  AddressRequest,
  ConstructorAddress,
} from '../../shared/module/address/address.module';
import { CommuneResponseModel } from '../../shared/module/commune/commune.module';
import { CustomerService } from '../../customer/customer.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { ModeratorService } from '../moderator.service';
import { NotificationComponent } from "../../shared/item/notification/notification.component";

@Component({
  selector: 'app-add-supplier',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AddressComponent,
    CommuneComponent,
    DistrictComponent,
    ProvinceComponent,
    NotificationComponent
],
  templateUrl: './add-supplier.component.html',
  styleUrl: './add-supplier.component.css',
})
export class AddSupplierComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAdd: boolean = false;
  supplier: SupplierRequestModerator = ContructorSupplierRequestModerator();

  //--
  addre: Address = ConstructorAddress();
  provinceSelect: number = 62;
  districtSelect: number = 1;
  communenInsert: CommuneResponseModel = {
    communeId: 1,
    communeName: '',
    districtId: 1,
  };

  //--
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(
    private service: CustomerService,
    private moderatorService: ModeratorService
  ) {}

  checkCategoryProperties() {
    this.isAdd = !!this.supplier.supplierName;
  }

  onDelete() {
    this.supplier = ContructorSupplierRequestModerator();
    this.checkCategoryProperties();
  }

  async onUpdate() {
    console.log(this.supplier);
    const addressRequest: AddressRequest = {
      communeId: this.communenInsert.communeId,
      communeName: this.communenInsert.communeName,
      districtId: this.districtSelect,
      houseNumber: this.addre.houseNumber,
      note: this.addre.note,
    };
    console.log('đang nhập địa chỉ mới!');
    console.log(addressRequest);

    // gọi api post address trả về id address gán vào addressId
    const response: BaseResponseModel = await this.service.postAddress(
      addressRequest
    );
    console.log(response);

    if (response.isSuccess) {
      this.supplier.addressId = response?.data;
      console.log(this.supplier);
      const response1: BaseResponseModel =
        await this.moderatorService.postSupplier(this.supplier);
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
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }

  //nhận 1 addres
  getAddressChangeChildComponent(address: Address) {
    this.addre = address;
    console.log(address);
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
