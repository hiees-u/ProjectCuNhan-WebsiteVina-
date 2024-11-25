import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  CategoryRequesModerator,
  ContructorCategoryModule,
} from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { ModeratorService } from '../moderator.service';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';

@Component({
  selector: 'app-add-cate',
  standalone: true,
  imports: [CommonModule, FormsModule, NotificationComponent],
  templateUrl: './add-cate.component.html',
  styleUrl: './add-cate.component.css',
})
export class AddCateComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAdd: boolean = false;
  category: CategoryRequesModerator = ContructorCategoryModule();

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private moderatorService: ModeratorService) {}

  checkCategoryProperties() {
    this.isAdd = !!this.category.categoryName;
  }

  onDelete() {
    this.category = ContructorCategoryModule();
    this.checkCategoryProperties();
  }

  async onUpdate() {
    console.log(this.category);
    const response = await this.moderatorService.postCategory(
      this.category.categoryName
    );
    if (response.isSuccess) {
      console.log('Thêm thành công!!');
      //
      this.dataNotification.status = 'success';
      this.dataNotification.messages =
        'Thêm Loại Sản phẩm thành công!!';
      this.trigger = Date.now();
    }
  }

  sendIsClose() {
    this.onDelete();
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
