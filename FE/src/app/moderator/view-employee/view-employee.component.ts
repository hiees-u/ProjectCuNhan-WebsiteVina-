import { Component, Input, SimpleChanges } from '@angular/core';
import { EmployeeRequestModule } from '../moderator.module';
import { CommonModule } from '@angular/common';
import { ModeratorService } from '../moderator.service';

@Component({
  selector: 'app-view-employee',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './view-employee.component.html',
  styleUrl: './view-employee.component.css',
})
export class ViewEmployeeComponent {
  @Input() deparmentID: number | undefined;

  customers: EmployeeRequestModule[] = [];

  totalPage: number = 1;
  pages: number[] = [];
  pageCurrent: number = 1;

  isError: boolean = false;

  constructor(private service: ModeratorService) {}

  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if(changes['deparmentID']) {
      this.getEmployee();
    }
  }

  ngOnInit(): void {
    this.getEmployee();
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
}
