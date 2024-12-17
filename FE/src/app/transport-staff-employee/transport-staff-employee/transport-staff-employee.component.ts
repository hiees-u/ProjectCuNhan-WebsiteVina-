import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OrderResponseModelTS } from '../transport-staff-employee.module';
import { TransportStaffEmployeeService } from '../transport-staff-employee.service';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';

@Component({
  selector: 'app-transport-staff-employee',
  imports: [CommonModule, CustomCurrencyPipe, NotificationComponent],
  templateUrl: './transport-staff-employee.component.html',
  styleUrl: './transport-staff-employee.component.css',
})
export class TransportStaffEmployeeComponent {
  constructor(
    private router: Router,
    private tsService: TransportStaffEmployeeService
  ) {}

  orders: OrderResponseModelTS[] = [];

  pageNumber: number = 0;
  pageSize: number = 0;

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  async ngOnInit() {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    await this.fetchOrders();
    console.log(this.orders);
  }

  logOutHandler() {
    this.router.navigate(['/login']);
    localStorage.removeItem('token');
  }

  async fetchOrders(): Promise<void> {
    try {
      const data = await this.tsService.fetchOrders();
      if (data.isSuccess) {
        this.orders = data.data;
      } else {
        console.error('Error fetching orders:', data.message);
      }
    } catch (error) {
      console.error('Error fetching orders:', error);
    }
  }

  async Success(ID: number) {
    this.trigger = Date.now();

    try {
      const data = await this.tsService.giaoHang(ID);

      if (data.isSuccess) {
        this.dataNotification.status = 'success';

        this.dataNotification.messages = `Đơn hàng #${ID} được giao thành công`;
        this.fetchOrders();
      }
    } catch {
      this.dataNotification.status = 'error';
      this.dataNotification.messages = `đã có lỗi xảy ra`;
    }
  }

  formatDate(dateInput: string | Date): string {
    const dateString =
      typeof dateInput === 'string' ? dateInput : dateInput.toISOString();
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${day}/${month}/${year} ${hours}:${minutes}`;
  }
}
