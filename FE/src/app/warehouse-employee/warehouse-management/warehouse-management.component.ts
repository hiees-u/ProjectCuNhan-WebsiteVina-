// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-warehouse-management',
//   standalone: true,
//   imports: [],
//   templateUrl: './warehouse-management.component.html',
//   styleUrl: './warehouse-management.component.css'
// })
// export class WarehouseManagementComponent {
  
// }
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-warehouse-management',
  templateUrl: './warehouse-management.component.html',
  styleUrls: ['./warehouse-management.component.css']
})
export class WarehouseManagementComponent implements OnInit {
  warehouses: any[] = [];

  constructor() {}

  ngOnInit(): void {
    // this.getWarehouses();
  }

  // getWarehouses(): void {
  //   this.warehouseEmployeeService.getWarehouses().subscribe(response => {
  //     if (response.isSuccess) {
  //       this.warehouses = response.data;
  //       console.log(this.warehouses);
        
  //     } else {
  //       console.error('Failed to fetch warehouses:', response.message);
  //     }
  //   }, error => {
  //     console.error('Error fetching warehouses:', error);
  //   });
  // }
}
