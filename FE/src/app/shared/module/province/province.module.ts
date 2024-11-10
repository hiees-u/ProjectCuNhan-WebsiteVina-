import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface ProvincesResponseModel {
  provinceId: number;
  provinceName: string;
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class ProvinceModule { }
