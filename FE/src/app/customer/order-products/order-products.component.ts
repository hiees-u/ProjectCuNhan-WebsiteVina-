import { Component } from '@angular/core';
import { CustomerService } from '../customer.service';
import { CartItem } from '../../shared/module/cart/cart.module';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import {
  Address,
  AddressRequest,
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
import {
  CommuneResponseModel,
} from '../../shared/module/commune/commune.module';
import {
  constructorOrderResponseModel,
  OrderRequestModule,
  OrderResponseModel,
  ProductQuantity,
} from '../../shared/module/order/order.module';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
    selector: 'app-order-products',
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
    styleUrl: './order-products.component.css'
})
export class OrderProductsComponent {
  data: CartItem[] = [];
  address: AddressString[] = [];
  addressSelectKey: number = 0;
  isShowInsertAddress: boolean = false;
  isPayment: boolean = false;
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
    districtId: 1,
  };
  
  //--
  Order: OrderRequestModule = {
    addressId: 0,
    nameRecipient: 'Hieu',
    phone: '101',
    paymentStatus: false,
    products: []
  };

  ResponseOrder: OrderResponseModel = constructorOrderResponseModel();

  //-- phương thức thanh toán
  // paymentMethod: boolean = false;
  
  constructor(private service: CustomerService, private router: Router) {}

  changePaymentMethod() {
    this.isPayment = !this.isPayment;
  }

  async onOrder() {
    if(this.isPayment) {
      console.log('Thanh toán QR trước');
      await this.showPayment();
      // gọi QR
      // nếu thành công thì tiếp tục
      this.Order.paymentStatus = true;
    }

    console.log('Thêm vào DB');
    await this.handleOrder();
  }

  async showPayment() {
    
  }

  async goToMomoPayment() {
    try {
        const orderInfo = {
            FullName: this.Order.nameRecipient,
            OrderId: Date.now().toString(),  // Tạo OrderId duy nhất
            OrderInfomation: 'Thanh toán đơn hàng cafe Vina',  // Nội dung đơn hàng
            Amount: this.ResponseOrder.totalPayment.toString(),  // Tổng tiền đơn hàng
            PaymentMethod: "vnpay" // Mặc định thanh toán qua VNPay
        };

        console.log('Dữ liệu gửi tới VNPay:', orderInfo); // Log dữ liệu gửi

        const vnpayResponse = await this.service.createPaymentMomo(orderInfo);

        if (vnpayResponse && vnpayResponse.payUrl) {
            console.log('URL thanh toán VNPay:', vnpayResponse.payUrl); // Log payUrl trả về
            window.location.href = vnpayResponse.payUrl; // Điều hướng tới URL thanh toán
        } else {
            throw new Error('Không thể tạo URL thanh toán VNPay!');
        }
    } catch (error: any) {
        console.error('Lỗi khi tạo thanh toán VNPay:', error.message);
        alert('Lỗi khi tạo thanh toán. Vui lòng thử lại!');
    }
}

  async handleOrder() {
    if (this.addressSelectKey === 0) {
      const addressRequest: AddressRequest = {
        communeId: this.communenInsert.communeId,
        communeName: this.communenInsert.communeName,
        districtId: this.districtSelect,
        houseNumber: this.addre.houseNumber,
        note: this.addre.note
      }
      console.log('đang nhập địa chỉ mới!');
      console.log(addressRequest);
      // gọi api post address trả về id address gán vào addressId
      const response: BaseResponseModel = await this.service.postAddress(addressRequest);
      console.log(response);
      
      if(response.isSuccess) {
        this.Order.addressId = response?.data;
      } else {
        this.trigger = Date.now();
        this.dataNotification = {
          messages: "Vui lòng thử lại",
          status: 'error'
        }
      }      
    } else {
      this.Order.addressId = this.addressSelectKey;
    }
    
    const orderProducts: ProductQuantity[] = [];

    console.log(this.data);
    

    this.data.forEach(item => {
      const product: ProductQuantity = {
        PriceHistoryId: item.priceHistoryId,
        Quantity: item.quantity
      }
      orderProducts.push(product);
    })

    //nếu thanh toán trước hoặc không
    this.Order.paymentStatus = this.isPayment;
    this.Order.products = orderProducts;

    console.log(this.Order);
    const response: BaseResponseModel = await this.service.postOrder(this.Order);
    console.log(response);
    
    if(response.isSuccess) {
      this.trigger = Date.now();
      this.dataNotification = {
        messages: "Đã đặt hàng thành công!",
        status: 'success'
      };
    } else {
      this.trigger = Date.now();
      this.dataNotification = {
        messages: "Vui lòng thử lại!",
        status: 'error'
      };
    }
  }

  onShowInsertAddress() {
    console.log(this.address);
    
    if (this.address.length <= 0) {
      this.isShowInsertAddress = true;
      console.log('SHOW ADDRESS INSERT');
      
    } else {
      this.isShowInsertAddress = !this.isShowInsertAddress;
    }

    if (this.isShowInsertAddress === false) {
      console.log('Ẩn Input thêm địa chỉ!');
      this.addressSelectKey = this.address[0].key;
    } else {
      console.log('Hiện Input thêm địa chỉ!');
      this.addressSelectKey = 0;
    }
  }

  async getUserInfo() {
    const response: BaseResponseModel = await this.service.getUserInfo();
    if (response.isSuccess) {
      this.Order.phone = response.data.phone;
      this.Order.nameRecipient = response.data.fullName;
      this.Order.addressId = response.data.addressId;
    }
  }

  async ngOnInit() {
    this.service.currentData.subscribe((data) => (this.data = data));
    this.data = this.data.filter((item) => item.quantity !== 0);
    this.data.forEach((item) => {
      this.ResponseOrder.totalPayment += item.totalPrice;
    });
    await this.getAddresses();
    console.log(this.address);

    if (this.address.length > 0) {
      this.addressSelectKey = this.address[0].key;
      console.log(this.addressSelectKey);
      
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
