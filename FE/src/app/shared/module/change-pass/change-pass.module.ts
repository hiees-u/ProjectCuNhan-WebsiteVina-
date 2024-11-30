import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface ChangePass {
  accountName: string;
  password: string;
  newPassword: string;
}

/*
{
  "accountName": "Hiu",
  "password": "123",
  "newPassword": "123@"
}
 */

export function ConstructorChangePass(): ChangePass {
  return {
    accountName: '',
    password: '',
    newPassword: '',
  };
}

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class ChangePassModule {}
