import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface OrderResponseModelTS {
  orderId: number;
  phone: string;
  addres: string;
  totalPayment: number;
  createAt: Date;
  paymentStatus: boolean;
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class TransportStaffEmployeeModule {}
