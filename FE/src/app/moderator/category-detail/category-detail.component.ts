import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CategoryRequesModerator, ContructorCategoryModule } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { ViewProductsComponent } from '../view-products/view-products.component';
import { ModeratorService } from '../moderator.service';
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";

@Component({
  selector: 'app-category-detail',
  standalone: true,
  imports: [
    FormsModule,
    ViewProductsComponent,
    NotificationComponent
],
  templateUrl: './category-detail.component.html',
  styleUrl: './category-detail.component.css'
})
export class CategoryDetailComponent {
  @Input() cate: CategoryRequesModerator = ContructorCategoryModule();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private service: ModeratorService) {}

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }

  async handleUpdate() {
    console.log(this.cate);
    const result: BaseResponseModel = await this.service.putCategory(this.cate);
    if(result.isSuccess) {
      this.dataNotification.status = 'success';
    } else {
      this.dataNotification.status = 'error';
    }
    this.dataNotification.messages = result.message!;
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây 
    setTimeout(() => { this.trigger = undefined; }, 30000);
  }

  async handleDelete() {
    console.log(this.cate);
    const result: BaseResponseModel = await this.service.deleteCategory(this.cate.categoryId);
    if(result.isSuccess) {
      this.dataNotification.status = 'success';
    } else {
      this.dataNotification.status = 'error';
    }
    this.dataNotification.messages = result.message!;
    this.trigger = Date.now();
    // Đặt lại `trigger` thành `undefined` sau 30 giây 
    setTimeout(() => { this.trigger = undefined; }, 30000);
  }
}
