import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ViewProductsComponent } from '../view-products/view-products.component';
import {
  ContructorSubCategoryModule,
  SubCategoryRequesModerator,
} from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import { ModeratorService } from '../moderator.service';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';

@Component({
    selector: 'app-sub-category-detail',
    imports: [FormsModule, ViewProductsComponent, NotificationComponent],
    templateUrl: './sub-category-detail.component.html',
    styleUrl: './sub-category-detail.component.css'
})
export class SubCategoryDetailComponent {
  @Input() subCate: SubCategoryRequesModerator = ContructorSubCategoryModule();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private service: ModeratorService) {}

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }

  async handleUpadte() {
    console.log(this.subCate);
    const result: BaseResponseModel = await this.service.putSubCategory(
      this.subCate
    );
    if (result.isSuccess) {
      this.dataNotification.status = 'success';
      this.dataNotification.messages = result.message!;
    } else {
      this.dataNotification.messages = 'Vui lòng kiểm tra lại..!';
      this.dataNotification.status = 'error';
    }
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây
    setTimeout(() => {
      this.trigger = undefined;
    }, 30000);
  }

  async handleDelete() {
    console.log(this.subCate);
    const result: BaseResponseModel = await this.service.deleteSubCategory(
      this.subCate.subCategoryId
    );
    if (result.isSuccess) {
      this.dataNotification.status = 'success';
      this.dataNotification.messages = result.message!;
      setTimeout(() => {
        this.trigger = undefined;
        this.sendIsClose();
      }, 3000)
    } else {
      this.dataNotification.status = 'error';
      this.dataNotification.messages = result.message!;
    }
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây
    setTimeout(() => {
      this.trigger = undefined;
    }, 30000);
  }
}
