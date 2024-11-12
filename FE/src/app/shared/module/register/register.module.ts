import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Register {
  accountName: string,
  password: string,
  rePassword: string
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class RegisterModule { }
