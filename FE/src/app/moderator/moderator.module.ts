import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';

import { FormsModule } from '@angular/forms';
import { ViewReportComponent } from './view-report/view-report.component';
//Customer type
export interface CustomerType {
  typeCustomerId: 0;
  typeCustomerName: '';
}

//customer request module
export interface CustomerRequestModule {
  accountName: string;
  fullName: string;
  email: string;
  phone: string;
  gender: number;
  typeCustomerId: number;
  typeCustomerName: string;
  addressId: number;
  addressString: string;
}

//employee
export interface EmployeeRequestModule {
  accountName: string;
  addressId?: number | null;
  addressName?: string | null;
  departmentId: number;
  departmentName: string;
  employId: number;
  employeeTypeId: number;
  employeeTypeName: string;
  fullName?: string | null;
  gender?: number | null;
}

// product-moderator.model.ts
export interface ProductModerator {
  productId: number;
  productName: string;
  image: string;
  totalQuantity: number;
  categoryId: number;
  supplier: number;
  subCategoryId: number;
  description: string;
  modifiedBy: string;
  price: number;
  priceHistoryId: number;
  subCategoryName: string;
  categoryName: string;
  expriryDate: string;
  createTime: string;
  modifiedTime: string;
  deleteTime: string | null;
}

//Supplier
export interface SupplierRequestModerator {
  supplierId: number;
  supplierName: string;
  addressId: number;
}

//Supplier response
export interface SupplierResponseModerator {
  supplierName: string;
  addressId: number;
}

//Department
export interface DepartmentRequestModerator {
  departmentId: number;
  departmentName: string;
  sumEmployee: number;
}

// products/product.model.ts
export interface InsertProduct {
  productName: string;
  image: File;
  categoryId: number;
  supplier: number;
  subCategoryId: number;
  expiryDate: string;
  description: string;
  price: number;
}

//category
export interface CategoryRequesModerator {
  categoryId: number;
  categoryName: string;
}

export interface SubCategoryRequesModerator {
  subCategoryId: number;
  subCategoryName: string;
}

export function ContructorCustomerRequestModule(): CustomerRequestModule {
  return {
    accountName: '',
    fullName: '',
    email: '',
    phone: '',
    gender: 0,
    typeCustomerId: 0,
    typeCustomerName: '',
    addressId: 0,
    addressString: '',
  };
}

export function ContructorDepartmentRequestModerator(): DepartmentRequestModerator {
  return {
    departmentId: 0,
    departmentName: '',
    sumEmployee: 0,
  };
}

export function ContructorCategoryModule(): CategoryRequesModerator {
  return {
    categoryId: 0,
    categoryName: '',
  };
}

export function ContructorSubCategoryModule(): SubCategoryRequesModerator {
  return {
    subCategoryId: 0,
    subCategoryName: '',
  };
}

export function ContructorSupplierRequestModerator(): SupplierRequestModerator {
  return {
    supplierId: 0,
    supplierName: '',
    addressId: 0,
  };
}

export function ContructorInsertProduct(): InsertProduct {
  return {
    productName: '',
    image: new File([], ''),
    categoryId: 0,
    supplier: 0,
    subCategoryId: 0,
    expiryDate: new Date().toISOString(),
    description: '',
    price: 0,
  };
}

export function ContructorProductModerator(): ProductModerator {
  return {
    productId: 0,
    productName: '',
    image: '',
    totalQuantity: 0,
    categoryId: 0,
    supplier: 0,
    subCategoryId: 0,
    description: '',
    modifiedBy: '',
    price: 0,
    priceHistoryId: 0,
    subCategoryName: '',
    categoryName: '',
    expriryDate: '',
    createTime: '',
    modifiedTime: '',
    deleteTime: '',
  };
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
  ],
})
export class ModeratorModule {}
