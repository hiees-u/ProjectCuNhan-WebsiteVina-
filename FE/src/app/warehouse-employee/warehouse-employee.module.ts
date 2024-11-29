import { NgModule } from '@angular/core';

// warehouse-employee.model.ts
export interface Warehouse {
  warehouseId: number;
  warehouseName: string;
  address: string;
  fullAddress: string;
  modifiedBy: string;
  createTime: string;
  modifiedTime: string;
  deleteTime: string | null;
}


// Constructor functions
export function ContructorWarehouse(): Warehouse {
  return {
    warehouseId: 0,
    warehouseName: '',
    address: '',
    fullAddress: '',
    modifiedBy: '',
    createTime: '',
    modifiedTime: '',
    deleteTime: null,
  };
}


@NgModule({
  declarations: [],
  imports: [],
})
export class WarehouseEmployeeModule {}
