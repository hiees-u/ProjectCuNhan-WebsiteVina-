import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';
import { ServicesService } from '../../services.service';
import { BaseResponseModel } from '../../module/base-response/base-response.module';
import { DistrictResponseModel } from '../../module/district/district.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-district',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './district.component.html',
  styleUrl: './district.component.css',
})
export class DistrictComponent {
  districts: DistrictResponseModel[] = [];

  @Input() selectedProvinceId: number = 0; //tinh
  @Input() selectedDistrictId: number = 0;
  @Output() selectedDistrictIdChange = new EventEmitter<number>();

  constructor(private service: ServicesService) {}

  async ngOnChanges(changes: SimpleChanges) {
    if (
      changes['selectedProvinceId'] &&
      !changes['selectedProvinceId'].isFirstChange()
    ) {
      if (this.selectedProvinceId !== 0) {
        const response: BaseResponseModel =
          await this.service.GetDistrictsByProvinceId(this.selectedProvinceId);
        if (response.isSuccess) {
          this.districts = response.data;
          if (this.districts.length > 0 && this.selectedDistrictId != 0) {
            this.selectedDistrictId = this.districts[0].districtId;
          }
          this.onDistrictChange();
        }
      }
    }
    if (changes['selectedDistrictId']) {
      // console.log('========================================');
      // console.log(
      //   'Huyện ID đã được nhận Component cha = ' + this.selectedDistrictId
      // );
      // console.log('thời gian: ', new Date().toLocaleString());
    }
  }

  async ngOnInit(): Promise<void> {
    // console.log('thời gian: ', new Date().toLocaleString());

    const response: BaseResponseModel = await this.service.GetDistricts();
    if (response.isSuccess) {
      this.districts = response.data;
    }
  }

  async onDistrictChange() {
    await this.selectedDistrictIdChange.emit(this.selectedDistrictId);
    // console.log('========================================');
    // console.log(
    //   'Huyện ID đã được trả Component cha = ' + this.selectedDistrictId
    // );
    // console.log('thời gian: ', new Date().toLocaleString());
  }
}
