import { Component } from '@angular/core';
import { ViewProductsComponent } from '../view-products/view-products.component';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './customer-dashboard.component.html',
  styleUrl: './customer-dashboard.component.css',
})
export class CustomerDashboardComponent {
  isLogin: boolean = true;
  isActive: number = 1;

  constructor(private router: Router) {
    this.checkLogin();
  }

  OnInit() {
    this.checkLogin();
  }

  checkLogin() {
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem('token');
      if (token) {
        this.isLogin = true;
      }
    }
    return false;
  }

  changeActive(activeNumber: number) {
    this.isActive = activeNumber;
  }

  navigateToViewProduct() {
    this.router.navigate(['/customer/view-product']);
  }

  navigateToOrderProduct() {
    this.router.navigate(['/customer/order-product']);
  }

  showCartDetail() {
    this.isActive = -1;
    this.router.navigate(['/customer/cart-details']);
  }

  showUserDetail() {
    console.log('show user detail');

    this.isActive = -1;
    if (this.isLogin) {
      this.router.navigate(['/customer/user-details']);
    } else {
      this.router.navigate(['/customer/view-product']);
    }
  }

  showLogin() {
    this.isActive = -1;
    this.router.navigate(['/login']);
  }

  showRegister() {
    this.isActive = -1;
    this.router.navigate(['/register']);
  }

  logOutHandler() {
    this.isActive = 3;
    this.router.navigate(['/customer/view-product']);
    this.isLogin = !this.isLogin;
    localStorage.removeItem('token');
  }
}
