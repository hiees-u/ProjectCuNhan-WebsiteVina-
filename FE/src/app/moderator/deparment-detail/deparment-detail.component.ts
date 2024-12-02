import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  ContructorDepartmentRequestModerator,
  DepartmentRequestModerator,
} from '../moderator.module';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { ModeratorService } from '../moderator.service';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ViewEmployeeComponent } from '../view-employee/view-employee.component';

@Component({
  selector: 'app-deparment-detail',
  imports: [
    NotificationComponent,
    CommonModule,
    FormsModule,
    ViewEmployeeComponent,
  ],
  templateUrl: './deparment-detail.component.html',
  styleUrl: './deparment-detail.component.css',
})
export class DeparmentDetailComponent {
  @Input() depar: DepartmentRequestModerator =
    ContructorDepartmentRequestModerator();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private service: ModeratorService) {}

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    console.log(this.depar);
  }

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }

  async handleUpdate() {
    console.log(this.depar);
    try {
      const response = await this.service.putDepartment(this.depar);
      console.log('Employee updated successfully', response);
      this.dataNotification.messages = 'Cập nhật Phòng Ban thành công..!';
      this.dataNotification.status = 'success';
    } catch (error) {
      console.error('Error updating employee', error);
      this.dataNotification.messages = 'Đã xảy ra lỗi. Thử lại sau..!';
      this.dataNotification.status = 'error';
    }
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây
    setTimeout(() => {
      this.trigger = undefined;
      this.sendIsClose();
    }, 5000);
  }

  async handleDelete() {
    try {
      const response = await this.service.deleteDepartment(this.depar.departmentId);
      console.log('Employee updated successfully', response);
      this.dataNotification.messages = 'Xóa Phòng Ban thành công..!';
      this.dataNotification.status = 'success';
    } catch (error) {
      console.error('Error updating employee', error);
      this.dataNotification.messages = 'Đã xảy ra lỗi. Thử lại sau..!';
      this.dataNotification.status = 'error';
    }
    this.trigger = Date.now();

    // Đặt lại `trigger` thành `undefined` sau 30 giây
    setTimeout(() => {
      this.trigger = undefined;
      this.sendIsClose();
    }, 5000);
  }
}
