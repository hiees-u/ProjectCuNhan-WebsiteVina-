import { Component } from '@angular/core';
import { ViewProductsComponent } from '../view-products/view-products.component';
import { NavigationEnd, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-customer-dashboard',
    imports: [CommonModule, RouterOutlet],
    templateUrl: './customer-dashboard.component.html',
    styleUrl: './customer-dashboard.component.css'
})
export class CustomerDashboardComponent {
  isLogin: boolean = true;
  isActive: number = 1;

  constructor(private router: Router) {
    this.checkLogin();
    this.router.events.subscribe(event => {
      if(event instanceof NavigationEnd) {
        this.checkLogin();
      }
    })
  }

  OnInit() {
    this.checkLogin();
  }

  checkLogin() {
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem('token');
      console.log('check login!!!');
      console.log(token);
      if (token === null) {
        console.log('token null');
        this.isLogin = false;
      } else {
        console.log('vào đây');
        this.isLogin = true;
      }
    }
    return false;
  }

  changeActive(activeNumber: number) {
    this.isActive = activeNumber;
  }

  navigateToContact() {
    // this.isActive = 4;
    this.router.navigate(['/customer/contact']);
  }

  navigateToViewProduct() {
    this.router.navigate(['/customer/view-product']);
  }

  navigateToAboutUs() {
    this.router.navigate(['/customer/about-us']);
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
    this.router.navigate(['/customer/about-us']);
    this.isLogin = !this.isLogin;
    localStorage.removeItem('token');
  }
}
