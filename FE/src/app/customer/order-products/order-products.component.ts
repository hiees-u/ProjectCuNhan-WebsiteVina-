import { Component } from '@angular/core';
import { CustomerService } from '../customer.service';
import { CartItem } from '../../shared/module/cart/cart.module';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';

@Component({
  selector: 'app-order-products',
  standalone: true,
  imports: [
    CommonModule,
    CustomCurrencyPipe
  ],
  templateUrl: './order-products.component.html',
  styleUrl: './order-products.component.css'
})
export class OrderProductsComponent {
  data: CartItem[] = [];

  constructor(private service: CustomerService) {}

  ngOnInit() {
    this.service.currentData.subscribe(data => this.data = data);
  }
  
  log() {
    console.log('dữ liệu đặt hàng:');    
    console.log(this.data);
  }
}
