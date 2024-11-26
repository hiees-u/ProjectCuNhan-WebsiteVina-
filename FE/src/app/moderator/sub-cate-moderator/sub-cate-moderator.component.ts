import { Component } from '@angular/core';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModeratorService } from '../moderator.service';
import { AddSubCateComponent } from '../add-sub-cate/add-sub-cate.component';
import { SubCategoryDetailComponent } from "../sub-category-detail/sub-category-detail.component";

@Component({
  selector: 'app-sub-cate-moderator',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AddSubCateComponent,
    SubCategoryDetailComponent
],
  templateUrl: './sub-cate-moderator.component.html',
  styleUrl: './sub-cate-moderator.component.css',
})
export class SubCateModeratorComponent {
  isShowAddSubCate: boolean | undefined;
  flag: boolean = false;

  flagDetail: boolean = false;
  isShowDetail: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';

  subCategorys: SubCategory[] = [];

  selectSubCate: SubCategory = {
    subCategoryId: 0,
    subCategoryName: '',
    image: '',
    deleteTime: new Date(),
    modifiedBy: '',
    createTime: new Date(),
    modifiedTime: new Date(),
    products: [],
  };

  //--

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    this.getSubCate();
  }

  handleClose(is: boolean) {
    console.log('THOÁT THÊM');
    this.getSubCate();
    this.isShowAddSubCate = !is;
    this.isShowDetail = !is;
    this.flag = true;
  }

  async getSubCate() {
    try {
      const response = await this.moderatorService.getSubCate(this.searchText);

      if (response.isSuccess) {
        this.subCategorys = response.data.data;
        this.totalPage = response.data.totalPages;

        console.log(this.subCategorys);
        console.log(this.totalPage);
      }
    } catch (error) {
      console.error('Error fetching subcategory: ', error);
    }
  }

  handlePageClick(page: number) {
    if (page === 1) {
      if (this.pageCurrent !== this.totalPage) this.pageCurrent += 1;
    } else {
      if (this.pageCurrent !== 1) this.pageCurrent -= 1;
    }
    this.getSubCate();
  }

  handleAddProduct() {
    this.flag = true;
    this.isShowAddSubCate = true;
  }

  onSearch() {
    console.log(this.searchText);
    this.getSubCate();
  }

  changeSubCateSelect(subCate: SubCategory) {
    console.log('HÍU Ở ĐÂY');
    
    this.selectSubCate = subCate;
    this.isShowDetail = true;
    this.flagDetail = true;
    console.log(this.selectSubCate);
  }
}
