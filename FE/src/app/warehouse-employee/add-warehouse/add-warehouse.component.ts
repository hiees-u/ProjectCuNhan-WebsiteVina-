import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WarehouseEmployeeService } from '../warehouse-employee.service';

@Component({
    selector: 'app-add-warehouse',
    imports: [CommonModule],
    templateUrl: './add-warehouse.component.html',
    styleUrl: './add-warehouse.component.css'
})
export class AddWarehouseComponent {

  constructor(private service: WarehouseEmployeeService) {}

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.get();
  }

  async get() {
    this.service.getWarehouses();
    console.log('tới đay');
    
  }

}
