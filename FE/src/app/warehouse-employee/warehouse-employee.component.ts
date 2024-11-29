import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-warehouse-employee',
  standalone: true,
  imports: [
    RouterModule
  ],
  templateUrl: './warehouse-employee.component.html',
  styleUrl: './warehouse-employee.component.css'
})
export class WarehouseEmployeeComponent {

  constructor(private router: Router) {}
  
  click_add_warehouse() {
    this.router.navigate(['/warehouse-employee/add-warehouse'])
  }
}
