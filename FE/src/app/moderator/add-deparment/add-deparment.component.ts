import { Component, EventEmitter, Output } from '@angular/core';
import { CategoryRequesModerator, ContructorCategoryModule } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-deparment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-deparment.component.html',
  styleUrl: './add-deparment.component.css'
})
export class AddDeparmentComponent {
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
