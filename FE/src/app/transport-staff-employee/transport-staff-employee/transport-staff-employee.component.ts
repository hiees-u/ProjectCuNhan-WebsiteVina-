import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OrderResponseModelTS } from '../transport-staff-employee.module';
import { TransportStaffEmployeeService } from '../transport-staff-employee.service';

@Component({
  selector: 'app-transport-staff-employee',
  imports: [],
  templateUrl: './transport-staff-employee.component.html',
  styleUrl: './transport-staff-employee.component.css',
})
export class TransportStaffEmployeeComponent {
  constructor(private router: Router, private tsService: TransportStaffEmployeeService) {}

  orders: OrderResponseModelTS[] = [];

  pageNumber: number = 0;
  pageSize: number = 0;

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.fetchOrders();
    console.log(this.orders);
    
  }

  logOutHandler() {
    this.router.navigate(['/login']);
    localStorage.removeItem('token');
  }

  async fetchOrders(): Promise<void> {
    try {
      const data = await this.tsService.fetchOrders();
      if (data.isSuccess) {
        this.orders = data.data;
      } else {
        console.error('Error fetching orders:', data.message);
      }
    } catch (error) {
      console.error('Error fetching orders:', error);
    }
  }
}
