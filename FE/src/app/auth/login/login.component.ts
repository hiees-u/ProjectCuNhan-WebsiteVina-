import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../auth.service';
import { Login } from '../../shared/module/login/login.module';

@Component({
    selector: 'app-login',
    imports: [FormsModule, CommonModule],
    templateUrl: './login.component.html',
    styleUrl: './login.component.css'
})
export class LoginComponent {
  // isLogin: boolean = false;
  response: string | undefined;

  loginData: Login = {
    accountName: '',
    password: '',
  };

  constructor(private router: Router, private authService: AuthService) {}

  async onLogin() {
    if (this.loginData.accountName && this.loginData.password) {
      localStorage.removeItem('token');
      try {
        const res = await this.authService.login(this.loginData);
        if (res.isSuccess) {
          // lưu vào localstorage
          localStorage.setItem('token', res.data as string);
          // login thành công
          const role = (await this.authService.getRole()).data;
          console.log('role => ', role);
          
          if (role == 0) {
            this.router.navigate(['/customer']);
          } else
          if ((await this.authService.getRole()).data == 2) {
            this.router.navigate(['/moderator']);
          } else
          if ((await this.authService.getRole()).data == 3) {
            this.router.navigate(['/order-approver']);
          } else
          if ((await this.authService.getRole()).data == 4) {
            this.router.navigate(['/warehouse-employee']);
          } 
          else {
            console.log('lỗi role=>', await this.authService.getRole());
          }
        } else {
          console.log(res.isSuccess + ' / ' + res.message);
          this.response = res.message;
        }
      } catch (error) {
        this.response = 'Kiểm tra lại mật khẩu..!';
        console.error('Login error:', error);
      }
    } else {
      this.response = 'Vui lòng nhập đầy đủ thông tin.';
      console.log(this.response);
    }
  }

  backToHomePage() {
    this.router.navigate(['/customer']);
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }
}
