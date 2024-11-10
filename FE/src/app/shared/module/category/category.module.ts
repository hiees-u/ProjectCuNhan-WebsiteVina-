import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../product/product.module';

export interface Category {
  categoryId: number;
  categoryName: string;
  image?: string;
  deleteTime?: Date;
  modifiedBy?: string;
  createTime: Date;
  modifiedTime?: Date;
  products: Product[];
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class CategoryModule {}
