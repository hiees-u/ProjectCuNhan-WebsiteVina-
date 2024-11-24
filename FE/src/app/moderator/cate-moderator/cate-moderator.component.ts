import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModeratorService } from '../moderator.service';
import { CategoryRequesModerator } from '../moderator.module';
import { AddCateComponent } from '../add-cate/add-cate.component';

@Component({
  selector: 'app-cate-moderator',
  standalone: true,
  imports: [CommonModule, FormsModule, AddCateComponent],
  templateUrl: './cate-moderator.component.html',
  styleUrl: './cate-moderator.component.css',
})
export class CateModeratorComponent {
  isShowAddCate: boolean | undefined;
  flag: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';

  categorys: CategoryRequesModerator[] = [];

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    this.getCategorys();
    // this.isShowAddCate = false;
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
    this.isShowAddCate = true;
  }

  handleClose(is: boolean) {
    console.log('THOÁT THÊM');
    this.getCategorys();
    this.isShowAddCate = !is;
    this.flag = true;
  }
}
