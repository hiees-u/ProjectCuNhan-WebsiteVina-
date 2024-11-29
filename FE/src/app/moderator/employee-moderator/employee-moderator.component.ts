import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModeratorService } from '../moderator.service';
import { EmployeeRequestModule } from '../moderator.module';
import { ViewEmployeeComponent } from "../view-employee/view-employee.component";

@Component({
  selector: 'app-employee-moderator',
  standalone: true,
  imports: [CommonModule, FormsModule, ViewEmployeeComponent],
  templateUrl: './employee-moderator.component.html',
  styleUrl: './employee-moderator.component.css',
})
export class EmployeeModeratorComponent {
  typeCustomerIdSelected: number | undefined;

  constructor(private moderatorService: ModeratorService) {}

  ngOnInit(): void {
    this.getEmployee() ;
  }

  onShowAddEmployee() {
    // this.isShowAddCate = true;
  }

  async getEmployee() {
    let employee: EmployeeRequestModule[] = [];
    let totalPages: number;
    await this.moderatorService.getEmployee(undefined, undefined, 1, 10)
    .then((data) => {
      employee = data.data.employees;
      totalPages = data.data.totalPages;
    })
  }
}
