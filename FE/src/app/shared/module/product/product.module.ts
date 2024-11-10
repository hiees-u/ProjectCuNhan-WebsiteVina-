import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Product {
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
  priceHistoryId: number,
  expriryDate: Date;
  createTime: Date;
  modifiedTime: Date;
  deleteTime: Date | null;
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class ProductModule {}
