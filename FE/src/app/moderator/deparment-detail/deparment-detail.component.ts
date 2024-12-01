import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ContructorDepartmentRequestModerator, DepartmentRequestModerator } from '../moderator.module';
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { ModeratorService } from '../moderator.service';
import { NotificationComponent } from "../../shared/item/notification/notification.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ViewEmployeeComponent } from "../view-employee/view-employee.component";

@Component({
    selector: 'app-deparment-detail',
    imports: [NotificationComponent, CommonModule, FormsModule, ViewEmployeeComponent],
    templateUrl: './deparment-detail.component.html',
    styleUrl: './deparment-detail.component.css'
})
export class DeparmentDetailComponent {
  @Input() depar: DepartmentRequestModerator = ContructorDepartmentRequestModerator();
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
    this.dataNotification.messages = '';
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây 
    setTimeout(() => { this.trigger = undefined; }, 30000);
  }

  async handleDelete() {
    this.dataNotification.messages = '';
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây 
    setTimeout(() => { this.trigger = undefined; }, 30000);
  }
}
