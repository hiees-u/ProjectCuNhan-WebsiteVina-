import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  CategoryRequesModerator,
  ContructorCategoryModule,
} from '../moderator.module';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-cate',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-cate.component.html',
  styleUrl: './add-cate.component.css',
})
export class AddCateComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAdd: boolean = false;
  category: CategoryRequesModerator = ContructorCategoryModule();

  checkCategoryProperties() {
    this.isAdd = !!this.category.categoryName;
  }

  onDelete() {
    this.category = ContructorCategoryModule();
    this.checkCategoryProperties();
  }

  onUpdate() {}

  sendIsClose() {
    this.onDelete();
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
