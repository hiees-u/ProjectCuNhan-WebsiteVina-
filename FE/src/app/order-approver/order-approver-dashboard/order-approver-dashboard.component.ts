import { Component } from '@angular/core';
import { OrderApproverService } from '../order-approver.service';
import { viewOrderApprover } from '../order-approver.module';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ModeratorService } from '../../moderator/moderator.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";
import { ViewOrderDetailComponent } from "../view-order-detail/view-order-detail.component";

@Component({
  selector: 'app-order-approver-dashboard',
  imports: [CommonModule, NotificationComponent, ViewOrderDetailComponent],
  templateUrl: './order-approver-dashboard.component.html',
  styleUrl: './order-approver-dashboard.component.css',
})
export class OrderApproverDashboardComponent {
  orders: viewOrderApprover[] = [];

  selectedOrder: number | undefined;

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();


  //~~~~~~~~~~~~~~~~~~~~~~
  isShowDetail: boolean = false;
  flag: boolean = false;

  constructor(
    private serviceOrderApprover: OrderApproverService,
    private router: Router,
    private moderatorService: ModeratorService
  ) {
    this.getAccountName();
  }

  ngOnInit(): void {
    this.getOrder();
  }

  onShowDetail(OrderId: number) {
    console.log(OrderId);
    this.selectedOrder = OrderId;
    this.isShowDetail = !this.isShowDetail;
    this.flag = true;
  }

  async getAccountName() {
    const result: BaseResponseModel = await this.moderatorService.getAccountName();

    if(result.isSuccess) {
      // this.accountName = result.data;
      console.log(result.data);
      this.dataNotification.messages = `Chào mừng bạn trở lại ${result.data}`;
      this.dataNotification.status = 'success';
      this.trigger = new Date();
    }
  }

  logOutHandler() {
    this.router.navigate(['/login']);
    localStorage.removeItem('token');
  }

  async getOrder() {
    try {
      this.orders = (await this.serviceOrderApprover.getOrderApprover()).data;
      console.log(this.orders);
    } catch (error) {
      console.error(error);
    }
  }

  formatDateTime(input: Date) {
    const date = new Date(input);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
  }
}
