import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModeratorService } from '../moderator.service';
import {
  DepartmentRequestModerator,
  EmployeeRequestModule,
} from '../moderator.module';
import { ViewEmployeeComponent } from '../view-employee/view-employee.component';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';

@Component({
    selector: 'app-employee-moderator',
    imports: [
        CommonModule,
        FormsModule,
        ViewEmployeeComponent,
        NotificationComponent,
    ],
    templateUrl: './employee-moderator.component.html',
    styleUrl: './employee-moderator.component.css'
})
export class EmployeeModeratorComponent {
  typeCustomerIdSelected: number | undefined;

  addSuccess: boolean = false;

  isShowAdd: boolean = false;
  flagAdd: boolean = false;

  newEmployee = {
    employeeTypeId: 6,
    departmentId: 7,
    accountName: '',
  };

  employeeTypes = [
    { id: 4, name: 'Quản trị kho' },
    { id: 5, name: 'Duyệt đơn' },
    { id: 6, name: 'Quản lý' },
  ];

  errorTextBoxAccount: boolean = false;

  departments: DepartmentRequestModerator[] = [];

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    // this.getEmployee();
    this.flagAdd = false;
    this.getDepartments();
  }

  async getDepartments() {
    this.moderatorService
      .getDeparment(1, 42)
      .then((data) => {
        this.departments = data.data.data;
      })
      .catch((error) => {
        console.error('Error fetching product', error);
      });
  }

  async handleAddEmployee() {
    if (this.newEmployee.accountName == '') {
      this.errorTextBoxAccount = true;
    } else {
      this.errorTextBoxAccount = false;
      this.moderatorService
        .postEmployee(this.newEmployee)
        .then((data) => {
          this.dataNotification.status = 'success';
          this.dataNotification.messages = data.message!;
          this.addSuccess = true;
        })
        .catch((error) => {
          this.dataNotification.status = 'error';
          this.dataNotification.messages = 'Kiểm tra lại thông tin';
        });
      this.trigger = Date.now();
      setTimeout(() => {
        this.onShowAddEmployee();
      }, 3000);
    }
    console.log(this.newEmployee);
  }

  onShowAddEmployee() {
    // this.isShowAddCate = true;
    this.flagAdd = true;
    this.isShowAdd = !this.isShowAdd;
    this.newEmployee.accountName = '';
    this.errorTextBoxAccount = false;
    // this.getEmployee();
  }

  // async getEmployee() {
  //   let employee: EmployeeRequestModule[] = [];
  //   let totalPages: number;
  //   await this.moderatorService
  //     .getEmployee(undefined, undefined, 1, 10)
  //     .then((data) => {
  //       employee = data.data.employees;
  //       totalPages = data.data.totalPages;
  //     });
  // }
}
