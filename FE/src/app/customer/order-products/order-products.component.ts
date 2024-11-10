import { Component } from '@angular/core';
import { CustomerService } from '../customer.service';
import { CartItem } from '../../shared/module/cart/cart.module';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import {
  Address,
  AddressString,
  ConstructorAddress,
} from '../../shared/module/address/address.module';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import { AddressComponent } from '../../shared/item/address/address.component';
import { CommuneComponent } from '../../shared/item/commune/commune.component';
import { DistrictComponent } from '../../shared/item/district/district.component';
import { ProvinceComponent } from '../../shared/item/province/province.component';
import { CommuneResponseModel, ConstructorCommune } from '../../shared/module/commune/commune.module';
import { constructorOrderResponseModel, OrderRequestModule, OrderResponseModel } from '../../shared/module/order/order.module';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-order-products',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    CustomCurrencyPipe,
    NotificationComponent,
    AddressComponent,
    CommuneComponent,
    DistrictComponent,
    ProvinceComponent,
  ],
  templateUrl: './order-products.component.html',
  styleUrl: './order-products.component.css',
})
export class OrderProductsComponent {
  data: CartItem[] = [];
  address: AddressString[] = [];
  addressSelectKey: number = 0;
  isShowInsertAddress: boolean = false;

  //--
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  //--
  addre: Address = ConstructorAddress();
  provinceSelect: number = 62;
  districtSelect: number = 1;
  communenInsert: CommuneResponseModel = {
    communeId: 1,
    communeName: '',
    districtId: 1
  }

  //--
  Order: OrderRequestModule = {
    addressId: 0,
    nameRecipient: 'Hieu',
    phone: '101'
  }

  ResponseOrder: OrderResponseModel = constructorOrderResponseModel();

  constructor(private service: CustomerService) {}

  onShowInsertAddress() {
    if(this.address.length <= 0)
      this.isShowInsertAddress = true;
    this.isShowInsertAddress = !this.isShowInsertAddress;
  }

  async getUserInfo() {
    const response: BaseResponseModel = await this.service.getUserInfo();
    if (response.isSuccess) {
      this.Order.phone = response.data.phone;
      this.Order.nameRecipient = response.data.fullName;
      this.Order.addressId = response.data.addressId;      
    }
  }

  ngOnInit() {
    this.service.currentData.subscribe((data) => (this.data = data));
    this.data = this.data.filter((item) => item.quantity !== 0);
    this.data.forEach(item => {
      this.ResponseOrder.totalPayment += item.totalPrice;
    })
    this.getAddresses();
    if (this.address.length > 0) {
      this.addressSelectKey = this.address[0].key;
    } else {
      this.onShowInsertAddress();
    }
    this.getUserInfo();
  }

  onSelectChange(event: any): void {
    this.addressSelectKey = event.target.value;
    console.log(this.addressSelectKey);
  }

  //function get danh sách
  async getAddresses() {
    const response: BaseResponseModel = await this.service.getStringAddresses();
    if (response.isSuccess) {
      this.address = response.data;
    }
    this.address.forEach((addres) => {
      console.log(addres);
    });
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

  log() {
    console.log('dữ liệu đặt hàng:');
    console.log(this.data);
  }
}
