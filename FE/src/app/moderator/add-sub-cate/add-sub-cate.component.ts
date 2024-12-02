import { Component, EventEmitter, Output } from '@angular/core';
import { ContructorSubCategoryModule, SubCategoryRequesModerator } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ModeratorService } from '../moderator.service';
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";

@Component({
    selector: 'app-add-sub-cate',
    imports: [
        CommonModule, FormsModule,
        NotificationComponent
    ],
    templateUrl: './add-sub-cate.component.html',
    styleUrl: './add-sub-cate.component.css'
})
export class AddSubCateComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAdd: boolean = false;
  subCategory: SubCategoryRequesModerator = ContructorSubCategoryModule();

  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private moderatorService: ModeratorService) {}

  checkCategoryProperties() {
    this.isAdd = !!this.subCategory.subCategoryName;
  }

  onDelete() {
    this.subCategory = ContructorSubCategoryModule();
    this.checkCategoryProperties();
  }

  async onUpdate() {
    console.log(this.subCategory);
    const response = await this.moderatorService.postSubCategory(
      this.subCategory.subCategoryName
    );
    if (response.isSuccess) {
      console.log('Thêm thành công!!');
      //
      this.dataNotification.status = 'success';
      this.dataNotification.messages =
        'Thêm Loại Sản phẩm phụ thành công!!';
      this.trigger = Date.now();
    }
  }

  sendIsClose() {
    this.onDelete();
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
