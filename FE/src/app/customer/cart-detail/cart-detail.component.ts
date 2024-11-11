import { Component } from '@angular/core';
import { CartItem, CartResponse } from '../../shared/module/cart/cart.module';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { CustomerService } from '../customer.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CustomCurrencyPipe,
    NotificationComponent
],
  templateUrl: './cart-detail.component.html',
  styleUrl: './cart-detail.component.css',
})
export class CartDetailComponent {

  isChecked = false;

  isDisabled: boolean = false;

  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  cartItems: CartItem[] = [];
  constructor(private Customer: CustomerService, private router: Router) {}

  ngOnInit(): void {
    this.getCart();     
  }

  async updateCart(productId: number, quantity: number) {
    const cartRequest: CartResponse = {
      productId,
      quantity
    }
    try {
      const response = await this.Customer.updateCart(cartRequest);
      if(response.isSuccess) {
        this.trigger = Date.now();
        this.dataNotification.messages = 'Cập nhật giỏ hàng thành công!';
        this.dataNotification.status = 'success';
        this.getCart();
      } else {
        this.trigger = Date.now();
        this.dataNotification.messages = 'Cập nhật giỏ hàng thất bại!';
        this.dataNotification.status = 'error';
      }
    } catch (error) {
      console.error('Error:', error);
    }
  }

  async deleteCart(productId: number) {
    try {
      const response = await this.Customer.deleteCart(productId);
      if(response.isSuccess) {
        this.dataNotification.messages = 'Xóa sản phẩm khỏi giỏ hàng thành công!';
        this.dataNotification.status = 'success';
        this.trigger = Date.now();
        this.getCart();
      } else {
        console.log('ERRROR');
      }
    } catch (error) {
      console.error('Error:', error);
    }
  }

  async getCart() {
    const response : BaseResponseModel = await this.Customer.getCart();
    if(response.isSuccess) {
      this.cartItems = response.data;
    }

    if(this.cartItems.length <= 0) {
      this.isDisabled = true;
    } else {
      this.isDisabled = false;
    }
    console.log(this.cartItems);
    
    console.log(this.isDisabled);
    
  }

  InDeCrease(productId: number, InDe: number) {
    this.cartItems.forEach(cart => {
      if(cart.productId === productId) {
        cart.quantity += InDe;
        // Đảm bảo số lượng không bao giờ nhỏ hơn 0
        if(cart.quantity <= 0) {
          cart.quantity = 0;
        }
        // Tính lại tổng giá sau khi thay đổi số lượng
        cart.totalPrice = cart.price * cart.quantity;
      }
    });
  }

  onCheckAll(event: any) {
    if(event.target.checked) {
      this.isChecked = true;
      this.cartItems.forEach(item => {
        item.checked = true;
      });
    } else {
      this.isChecked = false;
      this.cartItems.forEach(item => {
        item.checked = false;
      });
    }
  }
  
  onCheckboxChange(item: CartItem, event: any): void {
    item.checked = event.target.checked;
    console.log(item.productId + 'is' + (item.checked ? 'checked' : 'uncheck'));    
  }

  onCheckboxChangev2(item: CartItem) {
    item.checked = !item.checked;
  }

  onOrder() {
    let orderItems: CartItem[] = [];    

    this.cartItems.forEach(item => {
      if(item.checked) {
        item.totalPrice = item.price * item.quantity;
        orderItems.push(item);
        console.log(item.productId + 'is' + (item.checked ? 'checked' : 'uncheck'));   
      }
    });
    console.log(orderItems);

    //test
    this.Customer.sendData(orderItems);

    // chuyển sang trang đặt hàng
    this.router.navigate(['/customer/order-product']);
  }

}

