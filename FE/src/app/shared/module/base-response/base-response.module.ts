import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface BaseResponseModel {
  isSuccess: boolean;
  message?: string;
  data?: any;
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class BaseResponseModule { }

export function constructorBaseResponseModule() {
  return {
    isSuccess: true,
    message: '',
  }
}
