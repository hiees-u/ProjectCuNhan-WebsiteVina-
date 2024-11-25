import { Component, EventEmitter, Output } from '@angular/core';
import { ContructorSubCategoryModule, SubCategoryRequesModerator } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-sub-cate',
  standalone: true,
  imports: [
    CommonModule, FormsModule
  ],
  templateUrl: './add-sub-cate.component.html',
  styleUrl: './add-sub-cate.component.css'
})
export class AddSubCateComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAdd: boolean = false;
  subCategory: SubCategoryRequesModerator = ContructorSubCategoryModule();

  checkCategoryProperties() {
    this.isAdd = !!this.subCategory.subCategoryName;
  }

  onDelete() {
    this.subCategory = ContructorSubCategoryModule();
    this.checkCategoryProperties();
  }

  onUpdate() {
    console.log(this.subCategory);
  }

  sendIsClose() {
    this.onDelete();
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
