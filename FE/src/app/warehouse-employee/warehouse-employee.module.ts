import { NgModule } from '@angular/core';

// warehouse-employee.model.ts
export interface Warehouse {
  warehouseId: number;
  warehouseName: string;
  addressId: number;
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
    addressId: 0,
    fullAddress: '',
    modifiedBy: '',
    createTime: '',
    modifiedTime: '',
    deleteTime: null,
  };
}

export interface PostWareHouseRequestWarehouseEmployee {
  warehouseName: string;
  addressId: number;
}
export function ContructorPostWarehouseModule(): PostWareHouseRequestWarehouseEmployee {
  return {
    warehouseName: '',
    addressId: 0,
  };
}

export interface WareHouseRequestWarehouseEmployee {
  warehouseId: number;
  warehouseName: string;
  addressId: number;
}
export function ContructorRequestWarehouseModule(): WareHouseRequestWarehouseEmployee {
  return {
    warehouseId: 0,
    warehouseName: '',
    addressId: 0,
  };
}

@NgModule({
  declarations: [],
  imports: [],
})
export class WarehouseEmployeeModule {}


///===========Export Warehouse=============================
export interface DeliveryOrderDetail {
  orderId: number;
  priceHistoryId: number;
  quantity: number;
  cellId: number;
}

export function ContructorRequestDeliveryOrderDetailModule(): DeliveryOrderDetail {
  return {
    orderId: 0,
    priceHistoryId: 0,
    quantity: 0,
    cellId: 0
  };
}

export interface ExportWarehouseRequest {
  warehouseId: number;
  note: string;
  orderDetails: DeliveryOrderDetail[];
}

export function ContructorRequestExportWarehouseModule(): ExportWarehouseRequest {
  return {
    warehouseId: 0,
    note: '',
    orderDetails: []
  };
}


