import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ModeratorService } from '../moderator.service';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";

@Component({
    selector: 'app-add-deparment',
    imports: [CommonModule, FormsModule, NotificationComponent],
    templateUrl: './add-deparment.component.html',
    styleUrl: './add-deparment.component.css'
})
export class AddDeparmentComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAdd: boolean = false;
  departmentName: string = '';
  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private moderatorService: ModeratorService) {}

  checkCategoryProperties() {
    this.isAdd = !!this.departmentName;
  }

  onDelete() {
    this.departmentName = '';
    this.checkCategoryProperties();
  }

  async onUpdate() {
    console.log(this.departmentName);
    try {
      const response = await this.moderatorService.postDepartment(
        this.departmentName
      );

      if (response.isSuccess) {
        console.log('Thêm thành công!!');
        //
        this.dataNotification.status = 'success';
        this.dataNotification.messages = response.message!;
      } else {
        this.dataNotification.status = 'error';
        this.dataNotification.messages =
          'Có lỗi trong quá trình thêm phòng ban...!';
      }
      this.trigger = Date.now();
    } catch (error) {
      console.error('API LỖI POST DEPARTMENT!!');
      this.dataNotification.status = 'error';
      this.trigger = Date.now();
      this.dataNotification.messages =
        'Có lỗi trong quá trình thêm phòng ban...!';
    }
  }

  sendIsClose() {
    this.onDelete();
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
