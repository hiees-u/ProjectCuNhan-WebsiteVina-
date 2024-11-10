import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Register {
  userName: string,
  passWord: string,
  re_passWord: string
}


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class RegisterModule { }
