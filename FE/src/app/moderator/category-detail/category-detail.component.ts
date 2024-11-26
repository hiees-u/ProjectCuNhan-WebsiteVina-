import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CategoryRequesModerator, ContructorCategoryModule } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { ViewProductsComponent } from '../view-products/view-products.component';

@Component({
  selector: 'app-category-detail',
  standalone: true,
  imports: [
    FormsModule,
    ViewProductsComponent
  ],
  templateUrl: './category-detail.component.html',
  styleUrl: './category-detail.component.css'
})
export class CategoryDetailComponent {
  @Input() cate: CategoryRequesModerator = ContructorCategoryModule();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
