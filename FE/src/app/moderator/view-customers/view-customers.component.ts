import { CommonModule } from '@angular/common';
import { Component, Output } from '@angular/core';
import {
  ContructorCustomerRequestModule,
  CustomerRequestModule,
  CustomerType,
} from '../moderator.module';
import { ModeratorService } from '../moderator.service';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-view-customers',
    imports: [CommonModule, FormsModule],
    templateUrl: './view-customers.component.html',
    styleUrl: './view-customers.component.css'
})
export class ViewCustomersComponent {
  customers: CustomerRequestModule[] = [];

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;

  isError: boolean = false;

  customerTypes: CustomerType[] = [];

  constructor(private service: ModeratorService) {}

  ngOnInit(): void {
    this.getCustomer();
    this.getCustomerType();
  }

  //
  async handleSelectCusType(customer: CustomerRequestModule) {
    console.log(customer);
    this.service.putCustomer({accountName: customer.accountName, typeCustomerId: customer.typeCustomerId})
    .then((data) => {
      console.log(data);
    })
    .catch((error) => {
      console.log(error);
    })
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

  //API GET CUSTOMER TYPE
  async getCustomerType() {
    try {
      const response = await this.service.getCustomerType();
      if (response.isSuccess) {
        this.customerTypes = response.data;
      } else {
        console.error('Failed to load customer types:', response.message);
      }
    } catch (error) {
      console.error('Error loading customer types:', error);
    }
  }

  // handleSelectedCustomer(Customer: CustomerRequestModule) {
  //   this.selectedCustomer = Customer;
  //   // this.selectedCustomer.
  // }

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
