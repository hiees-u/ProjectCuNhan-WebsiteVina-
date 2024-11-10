import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Supplier {
  supplierId: number;
  supplierName: string;
  deleteBy?: string;
  deleteTime?: Date;
  address: string;
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class SupplierModule {}