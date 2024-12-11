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

///===========Export Warehouse=============================
export interface DeliveryOrderDetail {
  orderID: number;
  priceHistoryId: number;
  quantity: number;
  cellID: number;
}

export function ContructorRequestDeliveryOrderDetailModule(): DeliveryOrderDetail {
  return {
    orderID: 0,
    priceHistoryId: 0,
    quantity: 0,
    cellID: 0
  };
}

export interface ExportWarehouseRequest {
  warehouseId: number;
  note: string;
  deliveryNoteDetail: DeliveryOrderDetail[];
}

export function ContructorRequestExportWarehouseModule(): ExportWarehouseRequest {
  return {
    warehouseId: 0,
    note: '',
    deliveryNoteDetail: []
  };
}

// Get Order IDs
export interface OrderIDsResponse {
  orderId: number;
}

export function ContructorOrderIDsResponseModule(): OrderIDsResponse {
  return {
    orderId: 0
  };
}

//GET OrderDetail
export interface OrderDetailResponse {
  productId: number;
  productName: string;
  quantity: number;
  cellId: number;
  cellName: string;
  warehouseId: number;
  warehouseName: string;
  priceHistoryId: number;
}

export function ContructorOrderDetailResponseModule(): OrderDetailResponse {
  return {
    productId: 0,
    productName: '',
    quantity: 0,
    cellId: 0,
    cellName: '',
    warehouseId: 0,
    warehouseName: '',
    priceHistoryId: 0
  };
}


@NgModule({
  declarations: [],
  imports: [],
})
export class WarehouseEmployeeModule { }


