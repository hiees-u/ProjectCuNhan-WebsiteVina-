import { Component, Input, SimpleChanges } from '@angular/core';
import {
  DepartmentRequestModerator,
  EmployeeRequestModule,
} from '../moderator.module';
import { CommonModule } from '@angular/common';
import { ModeratorService } from '../moderator.service';
import { FormsModule } from '@angular/forms';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
    selector: 'app-view-employee',
    imports: [CommonModule, FormsModule, NotificationComponent],
    templateUrl: './view-employee.component.html',
    styleUrl: './view-employee.component.css'
})
export class ViewEmployeeComponent {
  @Input() deparmentID: number | undefined;
  @Input() flag: any;

  customers: EmployeeRequestModule[] = [];

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;

  isError: boolean = false;

  departments: DepartmentRequestModerator[] = [];

  isShowDelete: boolean = false;
  flagDelete: boolean = false;

  employeeTypes = [
    { id: 4, name: 'Quản trị kho' },
    { id: 5, name: 'Duyệt đơn' },
    { id: 6, name: 'Quản lý' },
  ];

  genders = [
    { id: 0, name: 'Nữ' },
    { id: 1, name: 'Nam' },
    // { id: null, name: 'Chưa xác định' },
  ];

  accountDelete: string = '';
  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  private subject: Subject<any> = new Subject();

  constructor(private service: ModeratorService) {
    this.subject
      .pipe(debounceTime(300))
      .subscribe({ next: (emp: any) => this.changeData(emp) });
  }

  onModelChange(emp: any) {
    this.subject.next(emp);
  }

  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if (changes['deparmentID']) {
      this.getEmployee();
    }
    if (changes['flag']) {
      console.log('load lại danh sách nhân viên');
      
      this.getEmployee();
    }
  }

  async changeData(emp: any) {
    const updateData = {
      accountName: emp.accountName,
      employeeTypeId: emp.employeeTypeId!,
      departmentId: emp.departmentId!,
      fullName: emp.fullName ?? null,
      gender: emp.gender ?? null,
    };
    console.log(updateData);
    try {
      const response = await this.service.putEmployee(updateData);
      console.log('Employee updated successfully', response);
    } catch (error) {
      console.error('Error updating employee', error);
    }
  }

  async handleDeleteEmployee(accountName: string) {
    console.log('xóa nhân viên', accountName);
    try {
      const response = await this.service.deleteEmployee(accountName);
      if (response.isSuccess) {
        console.log('Xóa thành công => ', accountName);
        this.dataNotification.status = 'success';
        this.getEmployee();
      } else {
        console.log('Xóa thất bại => ', accountName);
        this.dataNotification.status = 'error';
      }
      this.dataNotification.messages = response.message!;
      this.trigger = Date.now();
    } catch (error) {
      console.error(
        `Lỗi trong quá trình xóa nhân viên => ${accountName} `,
        error
      );
    }

    setTimeout(() => {
      this.onHidenDelete();
    }, 500);
  }

  async getDepartments() {
    this.service
      .getDeparment(1, 42)
      .then((data) => {
        this.departments = data.data.data;
      })
      .catch((error) => {
        console.error('Error fetching product', error);
      });
  }

  ngOnInit(): void {
    this.getEmployee();
    this.getDepartments();
  }

  async getEmployee() {
    console.log(this.deparmentID);
    await this.service
      .getEmployee(undefined, this.deparmentID, this.pageCurrent, 7)
      .then((data) => {
        this.customers = data.data.employees;
        this.totalPage = data.data.totalPages;
        this.pages = Array(this.totalPage)
          .fill(0)
          .map((x, i) => i + 1);
        console.log(this.customers);
        console.log(this.totalPage);
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
    this.getEmployee();
  }

  onShowDelete(accountName: string) {
    this.accountDelete = accountName;
    this.isShowDelete = true;
    this.flagDelete = true;
    console.log(this.accountDelete);
  }

  onHidenDelete() {
    this.isShowDelete = !this.isShowDelete;
    setTimeout(() => {
      this.accountDelete = '';
    }, 400);
  }
}
