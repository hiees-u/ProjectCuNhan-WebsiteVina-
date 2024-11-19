import { Component } from '@angular/core';
import { ModeratorService } from '../moderator.service';
import { ProductModerator } from '../moderator.module';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from "../../shared/module/customCurrency";
import { FormsModule } from '@angular/forms';
import { AddProductComponent } from "../add-product/add-product.component";

@Component({
  selector: 'app-product-moderator',
  standalone: true,
  imports: [
    CommonModule,
    CustomCurrencyPipe,
    FormsModule,
    AddProductComponent
],
  templateUrl: './product-moderator.component.html',
  styleUrl: './product-moderator.component.css',
})
export class ProductModeratorComponent {

  constructor(private service: ModeratorService) {}

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';
  isShowAddProduct: boolean = false;

  products: ProductModerator[] = [];
  flag: boolean = false;

  ngOnInit(): void {
    this.flag = false;
    this.getProduct();
  }

  onChangePage(page: number) {
    this.pageCurrent = page;
    this.getProduct();
  }

  onChangeSearch(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value;
    console.log(this.searchText);
  }

  onSearch() {
    console.log(this.searchText);
    this.getProduct();
  }
   
  getProduct(): void {
    this.service.getProducts(null,null,null, null, this.searchText, this.pageCurrent, 6, 0, 0)
      .then(data => {
        this.products = data.data.products;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage).fill(0).map((x,i) => i + 1);
        console.log(data.data);
        console.log(this.totalPage);        
        console.log(this.pages);        
      }).catch(error => {
        console.error('Error fetching product', error);
      })
  }

  handlePageClick(page: number) {
    if(page === 1)
    {
      if(this.pageCurrent !== this.totalPage)
        this.pageCurrent += 1;
    } else {
      if(this.pageCurrent !== 1)
        this.pageCurrent -= 1;
    }
    console.log(this.pageCurrent);
    this.getProduct();
  }

  handleAddProduct() {
    this.isShowAddProduct = true;
  }

  handleClose(is: boolean) {
    this.isShowAddProduct = !is;
    this.flag = true; 
  }
}
