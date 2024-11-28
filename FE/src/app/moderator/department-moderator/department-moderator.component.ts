import { Component } from '@angular/core';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DepartmentRequestModerator } from '../moderator.module';
import { ModeratorService } from '../moderator.service';
import { AddDeparmentComponent } from "../add-deparment/add-deparment.component";

@Component({
  selector: 'app-department-moderator',
  standalone: true,
  imports: [CommonModule, FormsModule, AddDeparmentComponent],
  templateUrl: './department-moderator.component.html',
  styleUrl: './department-moderator.component.css'
})
export class DepartmentModeratorComponent {
  isShowAddDepartment: boolean | undefined;
  flag: boolean = false;  

  isShowDetail: boolean | undefined;
  flagDetail: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';  

  subCategorys: SubCategory[] = [];

  //--
  deparments: DepartmentRequestModerator[] = [];

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    this.getDepartments();
  }

  onShowDetail() {
    this.isShowDetail = true;
    this.flagDetail = true;
  }

  onShowAddCate() {
    this.isShowAddDepartment = true;
    this.flag = true;
  }

  getDepartments() {
    this.moderatorService.getDeparment(1,42)
      .then(data => {
        this.deparments = data.data.data;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage).fill(0).map((x,i) => i + 1);
        console.log(this.deparments);
        console.log(this.pages);
        console.log(this.totalPage);     
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
    this.getDepartments();
  }

  onSearch() {
    console.log(this.searchText);
  }

  handleClose(is: boolean) {
    console.log('THOÁT THÊM');
    this.getDepartments();
    // this.getCategorys();
    this.isShowAddDepartment = !is;
    this.flag = true;
  }
}
