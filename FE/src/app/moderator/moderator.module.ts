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


@NgModule({
  declarations: [],
  imports: [],
})
export class ModeratorModule {}
