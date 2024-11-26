import { Component, Input, SimpleChanges } from '@angular/core';
import { ProductModerator } from '../moderator.module';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ModeratorService } from '../moderator.service';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-view-products',
  standalone: true,
  imports: [CommonModule, CustomCurrencyPipe, FormsModule],
  templateUrl: './view-products.component.html',
  styleUrl: './view-products.component.css',
})
export class ViewProductsComponent {
  constructor(
    private service: ModeratorService,
    private sanitizer: DomSanitizer
  ) {}

  @Input() cateId: number | null = null;
  @Input() subCateId: number | null = null;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  @Input() searchText: string = '';
  isShowAddProduct: boolean = false;
  flag: boolean = false;

  products: ProductModerator[] = [];

  isError: boolean = false;

  imageUrls: { [key: number]: SafeUrl } = {};
  defaultImageUrl: string = '/assets/Products/p1.png';

  ngOnInit(): void {
    this.flag = false;
    this.getProduct();

    this.products.forEach((item, index) => {
      this.checkImageExistence(`/assets/Products/${item.image}`, index);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if (changes['searchText']) {
      this.getProduct();
    }
    if (changes['cateId']) {
      console.log('Tới đây rồi: ',this.cateId);
      
      this.getProduct();
    }
    if(changes['subCateId']) {
      console.log('Tới đây rồi: ',this.subCateId);
      
      this.getProduct();
    }
  }

  checkImageExistence(url: string, index: number) {
    fetch(url, { method: 'HEAD' }).then((response) => {
      if (response.ok) {
        this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(url);
      } else {
        this.handleImageErrors(index, this.products[index].image);
      }
    });
  }

  onChangePage(page: number) {
    this.pageCurrent = page;
    this.getProduct();
  }

  onChangeSearch(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value;
  }

  // onSearch() {
  //   this.getProduct();
  // }

  getProduct(): void {
    this.service
      .getProducts(
        null,
        this.cateId,
        this.subCateId,
        null,
        this.searchText,
        this.pageCurrent,
        7,
        0,
        0
      )
      .then((data) => {
        this.products = data.data.products;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage)
          .fill(0)
          .map((x, i) => i + 1);
        console.log(this.products);
        if (this.products.length <= 0) this.isError = true; else this.isError = false;
      })
      .catch((error) => {
        console.error('Error fetching product', error);
      });
  }

  handlePageClick(page: number) {
    if (page === 1) {
      if (this.pageCurrent !== this.totalPage) this.pageCurrent += 1;
    } else {
      if (this.pageCurrent !== 1) this.pageCurrent -= 1;
    }
    this.getProduct();
  }

  async handleImageErrors(index: number, fileName: string): Promise<void> {
    const imageUrl = await this.service.getImage(fileName);
    if (imageUrl) {
      this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(imageUrl);
    } else {
      this.imageUrls[index] = this.sanitizer.bypassSecurityTrustUrl(
        this.defaultImageUrl
      );
    }
    console.log('Không tìm thấy ảnh??', this.imageUrls[index]);
  }
}
