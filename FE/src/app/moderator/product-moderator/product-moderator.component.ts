import { Component } from '@angular/core';
import { ModeratorService } from '../moderator.service';
import { ProductModerator } from '../moderator.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AddProductComponent } from '../add-product/add-product.component';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ViewProductsComponent } from '../view-products/view-products.component';

@Component({
  selector: 'app-product-moderator',
  standalone: true,
  imports: [CommonModule, FormsModule, AddProductComponent, ViewProductsComponent],
  templateUrl: './product-moderator.component.html',
  styleUrl: './product-moderator.component.css',
})
export class ProductModeratorComponent {
  constructor(
    // private service: ModeratorService,
    // private sanitizer: DomSanitizer
  ) {}

  // totalPage: number = 1;
  // pages: number[] = [];
  // pageCurrent: number = 1;
  searchText: string = '';
  isShowAddProduct: boolean = false;
  flag: boolean = false;

  // products: ProductModerator[] = [];

  // imageUrls: { [key: number]: SafeUrl } = {};
  // defaultImageUrl: string = '/assets/Products/p1.png';

  ngOnInit(): void {
    this.flag = false;
    // this.getProduct();

    // this.products.forEach((item, index) => {
    //   this.checkImageExistence(`/assets/Products/${item.image}`, index)
    // })
  }

  // checkImageExistence(url: string, index: number) {
  //   fetch(url, { method: 'HEAD' }).then((response) => {
  //     if (response.ok) {
  //       this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(url);
  //     } else {
  //       this.handleImageErrors(index, this.products[index].image)
  //     }
  //   });
  // }

  // onChangePage(page: number) {
  //   // this.pageCurrent = page;
  //   this.getProduct();
  // }

  onChangeSearch(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value;
  }

  onSearch() {
    // this.getProduct();
  }

  // getProduct(): void {
  //   this.service
  //     .getProducts(
  //       null,
  //       null,
  //       null,
  //       null,
  //       this.searchText,
  //       // this.pageCurrent,
  //       7,
  //       0,
  //       0
  //     )
  //     .then((data) => {
  //       this.products = data.data.products;
  //       this.totalPage = data.data.totalPages;
  //       this.pages = Array(this.totalPage)
  //         .fill(0)
  //         .map((x, i) => i + 1);
  //     })
  //     .catch((error) => {
  //       console.error('Error fetching product', error);
  //     });
  // }

  // handlePageClick(page: number) {
  //   if (page === 1) {
  //     if (this.pageCurrent !== this.totalPage) this.pageCurrent += 1;
  //   } else {
  //     if (this.pageCurrent !== 1) this.pageCurrent -= 1;
  //   }
  //   this.getProduct();
  // }

  handleAddProduct() {
    this.isShowAddProduct = true;
  }

  handleClose(is: boolean) {
    this.isShowAddProduct = !is;
    this.flag = true;
    // this.getProduct();
  }

  // async handleImageErrors(index: number, fileName: string): Promise<void> {
  //   const imageUrl = await this.service.getImage(fileName);
  //   if (imageUrl) {
  //     this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(imageUrl);
  //   } else {
  //     this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(this.defaultImageUrl);
  //   }
  //   console.log('Không tìm thấy ảnh??', this.imageUrls[index]);
  // }
}
