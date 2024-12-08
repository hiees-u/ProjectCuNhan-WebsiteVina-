import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface viewOrderApprover {
  orderId: number,
  phone: string,
  nameRecip: string,
  total: number,
  created: Date,
  createBy: string,
  address: string
}

export interface OrderDetailOA {
  priceHistory: number,
  name: string,
  image: string,
  gia: number,
  soLuongTon: number,
  soLuongMua: number,
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class OrderApproverModule {}
