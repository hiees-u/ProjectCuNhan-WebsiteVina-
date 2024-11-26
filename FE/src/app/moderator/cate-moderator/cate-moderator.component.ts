import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModeratorService } from '../moderator.service';
import { CategoryRequesModerator, ContructorCategoryModule } from '../moderator.module';
import { AddCateComponent } from '../add-cate/add-cate.component';
import { CategoryDetailComponent } from "../category-detail/category-detail.component";

@Component({
  selector: 'app-cate-moderator',
  standalone: true,
  imports: [CommonModule, FormsModule, AddCateComponent, CategoryDetailComponent],
  templateUrl: './cate-moderator.component.html',
  styleUrl: './cate-moderator.component.css',
})
export class CateModeratorComponent {
  isShowAddCate: boolean = false;
  flag: boolean = false;
  
  flagDetail: boolean = false;
  isShowDetail: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';

  categorys: CategoryRequesModerator[] = [];

  selectCate: CategoryRequesModerator = ContructorCategoryModule();

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    this.getCategorys();
    // this.isShowAddCate = false;
  }

  changCateSelect(cate: CategoryRequesModerator) {
    this.selectCate = cate;
    this.isShowDetail = true;
    this.flagDetail = true;
    console.log(this.selectCate);
  }

  async getCategorys() {
    try {
      const response = await this.moderatorService.getCate(this.searchText);

      if (response.isSuccess) {
        this.categorys = response.data.data;
        this.totalPage = response.data.totalPages;
      }
    } catch (error) {
      console.error('Error fetching categories:', error);
    }
  }

  handlePageClick(page: number) {
    if (page === 1) {
      if (this.pageCurrent !== this.totalPage) this.pageCurrent += 1;
    } else {
      if (this.pageCurrent !== 1) this.pageCurrent -= 1;
    }
    this.getCategorys();
  }

  onSearch() {
    this.getCategorys();
  }

  handleAddProduct() {
    this.flag = true;
    // this.flagDetail = true;
    this.isShowAddCate = true;
  }

  handleClose(is: boolean) {
    console.log('THOÁT THÊM');
    this.isShowDetail = !is;
    this.getCategorys();
    this.isShowAddCate = !is;
    this.flag = true;
    // this.flagDetail = true;
  }
}
