import { NgModule } from '@angular/core';

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

@NgModule({
  declarations: [],
  imports: [],
})
export class ModeratorModule {}
