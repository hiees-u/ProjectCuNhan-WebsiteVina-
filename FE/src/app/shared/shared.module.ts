import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ModelsService } from './models.service';
import { GuardsService } from './guards.service';
import { ServicesService } from './services.service';


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    ModelsService,
    GuardsService,
    ServicesService
  ]
})
export class SharedModule { }
