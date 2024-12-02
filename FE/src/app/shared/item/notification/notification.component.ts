import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ConstructerNotification,
  Notification,
} from '../../module/notification/notification.module';

@Component({
    selector: 'app-notification',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './notification.component.html',
    styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnChanges {
  @Input() data: Notification = ConstructerNotification();
  @Input() trigger: any; // Thuộc tính mới để kích hoạt thủ công
  lable: string = '';
  show = false;
  private timeOutId: any;

  ngOnChanges(changes: SimpleChanges) {
    // Kiểm tra nếu `trigger` thay đổi
    if (changes['trigger']) {
      this.showNotification();
      if (this.data.status === 'error') {
        this.lable = 'Lỗi';
      } else if (this.data.status === 'warning') {
        this.lable = 'Cảnh báo';
      } else if (this.data.status === 'success') {
        this.lable = 'Thành công';
      }
    }
    // console.log(this.data.status);
  }

  showNotification() {
    this.show = true;
    clearTimeout(this.timeOutId);
    // console.log('show', this.show);

    this.timeOutId = setTimeout(() => {
      this.show = false;
    }, 3000);
  }
}
