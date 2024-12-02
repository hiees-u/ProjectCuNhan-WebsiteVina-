import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AddProductComponent } from '../add-product/add-product.component';
import { ViewProductsComponent } from '../view-products/view-products.component';

@Component({
    selector: 'app-product-moderator',
    imports: [CommonModule, FormsModule, AddProductComponent, ViewProductsComponent],
    templateUrl: './product-moderator.component.html',
    styleUrl: './product-moderator.component.css'
})
export class ProductModeratorComponent {
  constructor(
    // private service: ModeratorService,
  ) {}
  searchText: string = '';
  isShowAddProduct: boolean = false;
  flag: boolean = false;
  uniqueKey = 0;

  ngOnInit(): void {
    this.flag = false;
  }

  onChangeSearch(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value;
  }

  onSearch() {
  }

  handleAddProduct() {
    this.isShowAddProduct = true;
  }

  handleClose(is: boolean) {
    this.isShowAddProduct = !is;
    this.flag = true;
    this.uniqueKey = Date.now();
  }
}
