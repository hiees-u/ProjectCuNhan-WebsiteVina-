import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { DepartmentRequestModerator } from '../moderator.module';
import { ModeratorService } from '../moderator.service';
import { ViewCustomersComponent } from "../view-customers/view-customers.component";

@Component({
    selector: 'app-customer-moderator',
    imports: [
        CommonModule,
        FormsModule,
        ViewCustomersComponent
    ],
    templateUrl: './customer-moderator.component.html',
    styleUrl: './customer-moderator.component.css'
})
export class CustomerModeratorComponent {
  // isShowAddCate: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';  

  subCategorys: SubCategory[] = [];

  //--
  deparments: DepartmentRequestModerator[] = [];

  //
  isShowDetail: boolean = false;

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    // this.getDepartments();
  }

  onShowAddCate() {
    // this.isShowAddCate = true;
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
