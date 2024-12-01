import { Component } from '@angular/core';
// import { NavigationEnd, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { ModeratorService } from '../moderator.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';

@Component({
    selector: 'app-moderator-dashboard',
    imports: [RouterOutlet, CommonModule],
    templateUrl: './moderator-dashboard.component.html',
    styleUrl: './moderator-dashboard.component.css'
})

export class ModeratorDashboardComponent {
  dashboardItem: number = 1;
  accountName: string = '';

  ngOnInit(): void {
    this.navigateToProduct();
    this.getAccountName();
  }

  constructor(private router: Router, private moderatorService: ModeratorService) { }

  async getAccountName() {
    const result: BaseResponseModel = await this.moderatorService.getAccountName();

    if(result.isSuccess) {
      this.accountName = result.data;
    }
  }

  changeDashboardItem(item: number) {
    this.dashboardItem = item;
  }

  navigateToCate() {
    this.changeDashboardItem(2);
    this.router.navigate(['/moderator/cate-moderator']);
  }
  navigateToProduct() {
    this.changeDashboardItem(1);
    this.router.navigate(['/moderator/product-moderator']);
  }
  navigateToSubCate() {
    this.changeDashboardItem(3);
    this.router.navigate(['/moderator/sub-cate-moderator']);
  }
  navigateToCustomer() {
    this.changeDashboardItem(4);
    this.router.navigate(['/moderator/customer-moderator']);
  }
  navigateToEmployee() {
    this.changeDashboardItem(5);
    this.router.navigate(['/moderator/employee-moderator']);
  }
  navigateToDepartment() {
    this.changeDashboardItem(6);
    this.router.navigate(['/moderator/department-moderator']);
  }
  navigateToSupplier() {
    this.changeDashboardItem(7);
    this.router.navigate(['/moderator/supplier-moderator']);
  }

  logOutHandler() {
    this.router.navigate(['/login']);
    localStorage.removeItem('token');
  }
}
