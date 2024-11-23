import { Component } from '@angular/core';
import { SubCategory } from '../../shared/module/sub-category/sub-category.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ModeratorService } from '../moderator.service';
import { DepartmentRequestModerator, SupplierRequestModerator } from '../moderator.module';

@Component({
  selector: 'app-supplier-moderator',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './supplier-moderator.component.html',
  styleUrl: './supplier-moderator.component.css'
})
export class SupplierModeratorComponent {
  isShowAddCate: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';  

  Suppliers: SupplierRequestModerator[] = [];

  //--

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    this.getSupplier();
  }

  onShowAddCate() {
    this.isShowAddCate = true;
  }

  getSupplier() {
    this.moderatorService.getSupplier(this.searchText)
      .then(data => {
        this.Suppliers = data.data.data;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage).fill(0).map((x,i) => i + 1);
        console.log(this.Suppliers);
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
    this.getSupplier();
  }

  onSearch() {
    console.log(this.searchText);
    this.getSupplier();
  }
}
