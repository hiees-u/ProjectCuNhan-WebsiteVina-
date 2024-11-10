import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface DistrictResponseModel {
  districtId: number;
  districtName: string;
  provinceId: number;
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class DistrictModule {}
