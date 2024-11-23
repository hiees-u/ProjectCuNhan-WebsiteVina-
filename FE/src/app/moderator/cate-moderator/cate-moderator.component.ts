import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Category } from '../../shared/module/category/category.module';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { ModeratorService } from '../moderator.service';
import { DepartmentRequestModerator } from '../moderator.module';

@Component({
  selector: 'app-cate-moderator',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './cate-moderator.component.html',
  styleUrl: './cate-moderator.component.css'
})
export class CateModeratorComponent {
 
  isShowAddCate: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';  

  subCategorys: SubCategory[] = [];

  //--
  deparments: DepartmentRequestModerator[] = [];

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    // this.getDepartments();
  }

  onShowAddCate() {
    this.isShowAddCate = true;
  }

  // getDepartments() {
  //   this.moderatorService.getDeparment(1,42)
  //     .then(data => {
  //       this.deparments = data.data.data;
  //       this.totalPage = data.data.totalPages;
  //       this.pages = Array(this.totalPage).fill(0).map((x,i) => i + 1);
  //       console.log(this.deparments);
  //       console.log(this.pages);
  //       console.log(this.totalPage);     
  //     }).catch(error => {
  //       console.error('Error fetching product', error);
  //     })
  // }

  handlePageClick(page: number) {
    if(page === 1)
    {
      if(this.pageCurrent !== this.totalPage)
        this.pageCurrent += 1;
    } else {
      if(this.pageCurrent !== 1)
        this.pageCurrent -= 1;
    }
    // this.getDepartments();
  }

  onSearch() {
    console.log(this.searchText);
  }
}
