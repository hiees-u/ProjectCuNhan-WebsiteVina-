import { Component } from '@angular/core';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModeratorService } from '../moderator.service';
import { DepartmentRequestModerator } from '../moderator.module';

@Component({
  selector: 'app-sub-cate-moderator',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './sub-cate-moderator.component.html',
  styleUrl: './sub-cate-moderator.component.css'
})
export class SubCateModeratorComponent {
 
  isShowAddCate: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';  

  subCategorys: SubCategory[] = [];

  //--

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    this.getSubCate();
  }

  onShowAddCate() {
    this.isShowAddCate = true;
  }

  async getSubCate() {
    try {
      const response = await this.moderatorService.getSubCate(this.searchText);

      if(response.isSuccess) {
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
    if(page === 1)
    {
      if(this.pageCurrent !== this.totalPage)
        this.pageCurrent += 1;
    } else {
      if(this.pageCurrent !== 1)
        this.pageCurrent -= 1;
    }
    this.getSubCate();
  }

  onSearch() {
    console.log(this.searchText);
    this.getSubCate();
  }
}