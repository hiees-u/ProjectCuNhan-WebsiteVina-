import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ViewProductsComponent } from '../view-products/view-products.component';
import { ContructorSubCategoryModule, SubCategoryRequesModerator } from '../moderator.module';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-sub-category-detail',
  standalone: true,
  imports: [
    FormsModule,
    ViewProductsComponent
  ],
  templateUrl: './sub-category-detail.component.html',
  styleUrl: './sub-category-detail.component.css',
})
export class SubCategoryDetailComponent {
  @Input() subCate: SubCategoryRequesModerator = ContructorSubCategoryModule();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
