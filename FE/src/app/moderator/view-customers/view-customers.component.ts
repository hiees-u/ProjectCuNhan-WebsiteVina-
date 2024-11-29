import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CustomerRequestModule } from '../moderator.module';
import { ModeratorService } from '../moderator.service';

@Component({
  selector: 'app-view-customers',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './view-customers.component.html',
  styleUrl: './view-customers.component.css',
})
export class ViewCustomersComponent {
  customers: CustomerRequestModule[] = [];

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;

  isError: boolean = false;

  constructor(private service: ModeratorService) {}

  ngOnInit(): void {
    this.getCustomer();
  }

  getCustomer() {
    this.service
      .getCustomer(undefined, this.pageCurrent, 7)
      .then((data) => {
        this.customers = data.data.customers;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage)
          .fill(0)
          .map((x, i) => i + 1);
        console.log(this.customers);
        if (this.customers.length <= 0) this.isError = true;
      })
      .catch((error) => {
        console.error(error);
      });
  }

  handlePageClick(page: number) {
    if (page === 1) {
      if (this.pageCurrent !== this.totalPage) this.pageCurrent += 1;
    } else {
      if (this.pageCurrent !== 1) this.pageCurrent -= 1;
    }
    //gọi lại danh sách khách hàng
    this.getCustomer();
  }
}
