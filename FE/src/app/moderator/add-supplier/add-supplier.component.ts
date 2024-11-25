import { Component, EventEmitter, Output } from '@angular/core';
import { CategoryRequesModerator, ContructorCategoryModule } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-supplier',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-supplier.component.html',
  styleUrl: './add-supplier.component.css'
})
export class AddSupplierComponent {
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
