import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Register } from '../../shared/module/register/register.module';
import { Router } from '@angular/router';
import { ConstructerNotification, Notification } from '../../shared/module/notification/notification.module';
import { NotificationComponent } from "../../shared/item/notification/notification.component";
import { AuthService } from '../auth.service';
import { BaseResponseModel, BaseResponseModule } from '../../shared/module/base-response/base-response.module';

@Component({
    selector: 'app-registre',
    imports: [FormsModule, CommonModule, NotificationComponent],
    templateUrl: './registre.component.html',
    styleUrl: './registre.component.css'
})
export class RegistreComponent {
  data: Register = {
    accountName: '',
    password: '',
    rePassword: '',
  };

  //--
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private router: Router, private service: AuthService) {}

  backOnLogin() {
    this.router.navigate(['/login']);
  }

  async onLogin() {
    if (this.data.accountName && this.data.password) {
      // Xử lý logic đăng nhập
      console.log('Username:', this.data.accountName);
      console.log('Password:', this.data.password);
      console.log('Re-password:', this.data.rePassword);
      // Thực hiện API call để kiểm tra đăng nhập
      const response: BaseResponseModel = await this.service.register(this.data);
      this.trigger = new Date();
      this.dataNotification.messages = response.message ? response.message : '';
      if(response.isSuccess) {
        this.dataNotification.status = 'success';
      }
      else {
        this.dataNotification.status = 'error'
      }
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
