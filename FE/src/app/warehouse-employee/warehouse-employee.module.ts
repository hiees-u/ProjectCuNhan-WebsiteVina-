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

//===========Import Warehouse=============================


export interface ReceiptDetailType {
  productID: number;
  cellID: number;
  quantity: number;
  purchaseOrderID: number;
}

export function ContructorRequestReceiptDetailTypeModule(): ReceiptDetailType {
  return {
    productID: 0,
    cellID: 0,
    quantity: 0,
    purchaseOrderID: 0
  };
}

export interface ImportWarehouseRequest {
  warehouseID: number;
  receiptDetails: ReceiptDetailType[];
}

export function ContructorRequestImportWarehouseModule(): ImportWarehouseRequest {
  return {
    warehouseID: 0,
    receiptDetails: []
  };
}

//Get purchaseOrderIds
export interface purchaseOrderIdsResponse {
  purchaseOrderId: number;
}

export function ContructorPurchaseOrderIdsResponseModule(): purchaseOrderIdsResponse {
  return {
    purchaseOrderId: 0
  };
}

//Get purchaseOrderDetail
export interface PurchaseOrderDetailResponse {
  productId: number;
  productName: string;
  quantityOrdered: number;
  quantityDelivered: number;
  cellId: number;
  cellName: string;
  priceHistoryId: number;
  price: number;
  quantityToImport: number;
}
export function ContructorPurchaseOrderDetailResponseModule(): PurchaseOrderDetailResponse {
  return {
    productId: 0,
    productName: '',
    quantityOrdered: 0,
    quantityDelivered: 0,
    cellId: 0,
    cellName: '',
    priceHistoryId: 0,
    price: 0,
    quantityToImport: 1
  };
}

export interface ProductsExpriryDate {
  productId: number;
  productName: string;
  image: string;
  modifiedBy: string;
  expriryDate: Date;
  createTime: string;
  modifiedTime: string;
}
export function ContructorProductsExpriryDateResponseModule(): ProductsExpriryDate {
  return {
    productId: 0,
    productName: '',
    image: '',
    modifiedBy: '',
    expriryDate: new Date(),
    createTime: '',
    modifiedTime: '',
  };
}

//LIST PRODUCT
export interface Products{
  productId: number;
  productName: string;
}
export function ContructorProductsResponseModule(): Products {
  return {
    productId: 0,
    productName: ''
  };
}

//InfoProduct In Warehouse
export interface InfoProducts {
  productName: string;
  image: string;
  warehouseName: string;
  cellName: string;
  shelvesName: string;
  totalQuantity: number;
  expriryDate: Date;
}
export function ContructorInfoProductsResponseModule(): InfoProducts {
  return {
    productName: '',
    image: '',
    warehouseName: '',
    cellName: '',
    shelvesName: '',
    totalQuantity: 0,
    expriryDate: new Date()
  };
}

@NgModule({
  declarations: [],
  imports: [],
})
export class WarehouseEmployeeModule { }


