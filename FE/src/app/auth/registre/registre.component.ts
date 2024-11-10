import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Register } from '../../shared/module/register/register.module';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registre',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './registre.component.html',
  styleUrl: './registre.component.css'
})
export class RegistreComponent {
  loginData: Register = {
    userName: '',
    passWord: '',
    re_passWord: ''
  };

  constructor(private router: Router) {}

  backOnLogin() {
    this.router.navigate(['/login']);
  }

  onLogin() {
    if (this.loginData.userName && this.loginData.passWord) {
      // Xử lý logic đăng nhập
      console.log('Username:', this.loginData.userName);
      console.log('Password:', this.loginData.passWord);
      console.log('Re-password:', this.loginData.re_passWord);
      // Thực hiện API call để kiểm tra đăng nhập
      this.router.navigate(['/login']);
    } else {
      console.log('Vui lòng nhập đầy đủ thông tin.');
    }
  }

  backToHomePage() {
    this.router.navigate(['/customer']);
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
