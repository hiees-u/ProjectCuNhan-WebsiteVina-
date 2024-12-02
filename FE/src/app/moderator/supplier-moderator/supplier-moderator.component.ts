import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ModeratorService } from '../moderator.service';
import {
  ContructorSupplierRequestModerator,
  SupplierRequestModerator,
} from '../moderator.module';
import { AddSupplierComponent } from '../add-supplier/add-supplier.component';
import { SupplierDetailComponent } from "../supplier-detail/supplier-detail.component";

@Component({
    selector: 'app-supplier-moderator',
    imports: [CommonModule, FormsModule, AddSupplierComponent, SupplierDetailComponent],
    templateUrl: './supplier-moderator.component.html',
    styleUrl: './supplier-moderator.component.css'
})
export class SupplierModeratorComponent {
  isShowAddSupplier: boolean | undefined;
  flag: boolean = false;

  flagDetail: boolean = false;
  isShowDetail: boolean = false;

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;
  searchText: string = '';

  Suppliers: SupplierRequestModerator[] = [];

  selectSupplier: SupplierRequestModerator =
    ContructorSupplierRequestModerator();

  //--

  constructor(private moderatorService: ModeratorService) {}

  onChangeSelectedSuppler(selectSupplier: SupplierRequestModerator) {
    this.selectSupplier = selectSupplier;
    this.isShowDetail = true;
    this.flagDetail = true;
    console.log(this.selectSupplier);
  }

  ngOnInit(): void {
    this.getSupplier();
  }

  onShowAddCate() {
    this.flag = true;
    this.isShowAddSupplier = true;
  }

  getSupplier() {
    this.moderatorService
      .getSupplier(this.searchText)
      .then((data) => {
        this.Suppliers = data.data.data;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage)
          .fill(0)
          .map((x, i) => i + 1);
        console.log(this.Suppliers);
        console.log(this.pages);
        console.log(this.totalPage);
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
    this.getSupplier();
  }

  onSearch() {
    console.log(this.searchText);
    this.getSupplier();
  }

  handleClose(is: boolean) {
    console.log('THOÁT THÊM');
    // this.getCategorys();
    this.isShowAddSupplier = !is;
    this.isShowDetail = !is;
    this.flag = true;
    this.getSupplier();
  }
}
