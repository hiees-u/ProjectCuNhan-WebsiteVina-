import { Component } from '@angular/core';
import { CustomerService } from '../customer.service';
import {
  BaseResponseModel,
  BaseResponseModule,
} from '../../shared/module/base-response/base-response.module';
import {
  ConstructerUserInfoResponseModel,
  UserInfoResponseModel,
} from '../../shared/module/user-info/user-info.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProvinceComponent } from '../../shared/item/province/province.component';
import { DistrictComponent } from '../../shared/item/district/district.component';
import { CommuneComponent } from '../../shared/item/commune/commune.component';
import { AddressComponent } from '../../shared/item/address/address.component';
import {
  Address,
  ConstructorAddress,
} from '../../shared/module/address/address.module';
import { ServicesService } from '../../shared/services.service';
import {
  CommuneResponseModel,
  ConstructorCommune,
} from '../../shared/module/commune/commune.module';
import { UserInfoRequestModel } from '../../shared/module/user-info/user-info.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { OrderDetailModel } from '../../shared/module/order/order.module';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { Router } from '@angular/router';
import { ModeratorService } from '../../moderator/moderator.service';
import {
  ChangePass,
  ConstructorChangePass,
} from '../../shared/module/change-pass/change-pass.module';

@Component({
  selector: 'app-user-detail',
  imports: [
    CommonModule,
    FormsModule,
    ProvinceComponent,
    DistrictComponent,
    CommuneComponent,
    AddressComponent,
    NotificationComponent,
    CustomCurrencyPipe,
  ],
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css', './user-detail.component-v2.css'],
})
export class UserDetailComponent {
  addressString: string = '';
  isShowCommune: Boolean = false;
  isActive: number = 1;
  address: Address = ConstructorAddress();
  userInfo: UserInfoResponseModel = ConstructerUserInfoResponseModel();
  communenInsert: CommuneResponseModel = ConstructorCommune(); //-- lưu thông tin Commune(xã) được thêm mới

  //--
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  //--
  underlineTransform = 'translateX(0%)';
  underActive = 0;
  Active: number = -1;
  orders: OrderDetailModel[] = [];

  //---
  changePass: ChangePass = ConstructorChangePass();

  //---
  isShowConfirmChangePass: boolean = false;

  //--
  isShowConfirmDelete: boolean = false;

  time: boolean = false;

  //--
  ischangeAddress: boolean = false;

  constructor(
    private service: CustomerService,
    private servicee: ServicesService,
    private serviceModerator: ModeratorService,
    private router: Router
  ) {}

  async ngOnInit() {
    console.log('tới đây..?');

    await this.getUserInfo();
    if (!this.userInfo.accountName) {
      console.log('del co ten dang nhap');
      this.getAccountName();
    }
    console.log(this.userInfo);
    await this.getOrder(-1);
    console.log(this.orders);

    console.log('SHow', this.isShowConfirmChangePass);
  }

  onChangeAddress() {
    this.ischangeAddress = !this.ischangeAddress;
  }

  async onChangePassword() {
    const response = await this.service.changePassword(this.changePass);

    if (response.isSuccess) {
      this.dataNotification.messages = 'Đổi mật khẩu thành công...!';
      this.dataNotification.status = 'success';

      setTimeout(() => {
        this.router.navigate(['/login']);
        localStorage.removeItem('token');
      }, 5000);
    } else {
      this.dataNotification.messages = 'Vui lòng xử lý lại..!';
      this.dataNotification.status = 'error';
    }
    this.trigger = Date.now();
  }

  onShowConfirmChangePass() {
    this.isShowConfirmChangePass = !this.isShowConfirmChangePass;
  }

  async getAccountName() {
    await this.serviceModerator.getAccountName().then((data) => {
      this.userInfo.accountName = data.data;
    });
  }

  async getAddressById(idAddress: number) {
    const response: BaseResponseModel = await this.servicee.GetAddressById(
      idAddress
    );
    if (response.isSuccess) {
      console.log('UserInfo có data: ', new Date().toLocaleString);
      this.address = response.data;
    }
  }

  // Fetch PUT UserInfo
  async isHandleUpdate() {
    console.log(this.userInfo);
    console.log(this.address);
    console.log(this.communenInsert);

    // chuẩn bị:
    const request: UserInfoRequestModel = {
      fullName: this.userInfo.fullName,
      addressId: this.address.addressId,
      commune:
        this.communenInsert.communeId != -1
          ? this.communenInsert.communeId
          : this.userInfo.commune,
      communeName: this.communenInsert.communeName,
      districtId: this.userInfo.district!,
      email: this.userInfo.email,
      gender:
        typeof this.userInfo.gender === 'string'
          ? parseInt(this.userInfo.gender, 10)
          : 0,
      houseNumber: this.address.houseNumber,
      note: this.address.note,
      phone: this.userInfo.phone,
    };

    console.log(request);
    console.log('Commune Id đang được selected: ', this.userInfo.commune);

    // chạy:
    const response: BaseResponseModel = await this.service.putUserInfo(request);
    this.dataNotification.messages = response.message!;
    if (response.isSuccess) {
      this.dataNotification.status = 'success';
    } else {
      this.dataNotification.status = 'warning';
    }
    this.trigger = Date.now();
    console.log(response);
  }

  getCommutNameChangeChildComponent(commune: CommuneResponseModel) {
    this.communenInsert = commune;
    console.log(commune);
    console.log(this.communenInsert);
  }

  //nhận 1 addres
  async getAddressChangeChildComponent(address: Address) {
    this.address = address;
    console.log(address);
  }

  //nhận huyện id đang được selected từ component con
  async getDistrictIDChangeChildComponent(event: number) {
    if (this.userInfo.district !== event) {
      this.userInfo.district = event;
    }
  }

  //nhận tỉnh id đang được selected từ component con
  async getProvinceIDChangeChildComponent(provinceId: number) {
    this.userInfo.province = provinceId;
  }

  //-- Gọi API lấy dữ liệu UserInfo
  async getUserInfo() {
    const response: BaseResponseModel = await this.service.getUserInfo();
    console.log('Lấy thông tin khách hàng...');

    if (response.isSuccess) {
      console.log(this.userInfo);

      this.userInfo = await response.data;

      this.isShowCommune = true;

      this.time = true;

      if (this.userInfo.addressId) {
        console.log('get address');

        await this.getAddressById(this.userInfo.addressId);
      }
    }
  }

  changeActive(number: number) {
    this.isActive = number;
    this.getOrder(-1);
    this.changePass.accountName = this.userInfo.accountName;
  }

  logOutHandler() {
    // this.isActive = 3;
    this.router.navigate(['/customer/about-us']);
    localStorage.removeItem('token');
  }

  moveUnderline(index: number): void {
    this.underActive = index;
    const percentage = index * 100;
    this.underlineTransform = `translateX(${percentage}%)`;
  }

  //-- select Order
  async getOrder(oredrState: number | undefined) {
    if (oredrState === undefined) oredrState = this.Active;
    else this.Active = oredrState;
    const response: BaseResponseModel = await this.service.getOrder(oredrState);
    if (response.isSuccess) {
      this.orders = response.data;
    }
    console.log(this.orders);
  }

  async deleteOrder() {
    console.log('OrderId: ', this.selectOderId);
    console.log('priceHistoryId: ', this.selectPriceHistoryId);
    const response: BaseResponseModel = await this.service.deleteOrder(
      this.selectOderId!,
      this.selectPriceHistoryId!
    );
    this.trigger = new Date();
    this.dataNotification.messages = response.message ? response.message : '';
    if (response.isSuccess) {
      this.dataNotification.status = 'success';
      console.log('lấy danh sách order mới!!!');
      this.getOrder(undefined);
      setTimeout(() => {
        this.onShowConfirmDelete(0,0);
      },3000);
    } else {
      this.dataNotification.status = 'error';
    }
  }

  selectOderId: number | undefined;
  selectPriceHistoryId: number | undefined;

  onShowConfirmDelete(orderId: number, priceHistoryId: number) {
    this.selectOderId = orderId;
    this.selectPriceHistoryId = priceHistoryId;
    this.isShowConfirmDelete = !this.isShowConfirmDelete;
    console.log(this.isShowConfirmDelete);
  }
}
