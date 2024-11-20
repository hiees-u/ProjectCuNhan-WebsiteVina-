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

export function ContructorInsertProduct():InsertProduct {
  return {
    productName: '',
    image: new File([], ''),
    categoryId: 0,
    supplier: 0,
    subCategoryId: 0,
    expiryDate: new Date().toISOString(),
    description: '',
    price: 0,
  }
}


@NgModule({
  declarations: [],
  imports: [],
})
export class ModeratorModule {}
