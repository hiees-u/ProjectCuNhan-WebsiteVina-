import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { ServicesService } from '../../services.service';
import { ProvincesResponseModel } from '../../module/province/province.module';
import { BaseResponseModel } from '../../module/base-response/base-response.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-province',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './province.component.html',
  styleUrl: './province.component.css'
})
export class ProvinceComponent {
  provinces: ProvincesResponseModel[] = [];
  @Input() selectedProvinceId: number = 0;
  @Output() selectedProvinceIdChange = new EventEmitter<number>();
  constructor(private service: ServicesService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['selectedProvinceId']) {
      // console.log('========================================');
      // console.log('Tỉnh ID đã được truyền vào Province Component = '+ this.selectedProvinceId);
      // console.log('thời gian: ' , new  Date().toLocaleString());
    }
  }

  async ngOnInit(): Promise<void> {
    // console.log('========================================');    
    // console.log('Khởi Tạo Province!!', new Date().toLocaleString());
    
    const response : BaseResponseModel = await this.service.GetProvinces();
    if(response.isSuccess) {
      this.provinces = response.data;
    }
  }

  onProvinceChange() {
    // console.log('========================================');    
    // console.log('Tỉnh ID đang seleceted đã được gửi đi từ Province Component = '+ this.selectedProvinceId);
    //   console.log('thời gian: ' , new  Date().toLocaleString());
    this.selectedProvinceIdChange.emit(this.selectedProvinceId);
  }
}
