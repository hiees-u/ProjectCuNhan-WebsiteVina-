import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  CommuneResponseModel,
  ConstructorCommune,
} from '../../module/commune/commune.module';
import { ServicesService } from '../../services.service';
import { BaseResponseModel } from '../../module/base-response/base-response.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-commune',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './commune.component.html',
  styleUrl: './commune.component.css',
})
export class CommuneComponent {
  @Input() selectedCommuneId: number = 0;
  @Input() selectedDistrictId: number = 0;
  @Input() communeInput: CommuneResponseModel = ConstructorCommune();
  @Output() communeOutput = new EventEmitter<CommuneResponseModel>();
  //--
  isShowInput: boolean = false;

  commune: CommuneResponseModel[] = [];

  constructor(private service: ServicesService) {}

  ngOnInit(): void {
    this.getComunesByDistrict(this.selectedDistrictId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedCommuneId']) {
      console.log('Dữ liệu Xã Id được thay đổi: ', this.selectedCommuneId);
    }

    if (changes['selectedDistrictId']) {
      console.log('Dữ liệu Huyện Id được thay đổi: ', this.selectedDistrictId);
      this.getComunesByDistrict(this.selectedDistrictId);
    }
  }

  //khi select commune or input commune is send data to parent
  onCommuneChange() {
    if (this.isShowInput) {
      console.log('hiện input thêm xã');
      this.communeInput.communeId = 0;
    } else {
      let communeId = this.selectedCommuneId;
      if (typeof communeId === 'string') {
        communeId = parseInt(communeId, 10);
      }
      // this.communeInput.communeId = communeId;
      this.communeInput = this.commune.find(
        (com) => com.communeId === communeId
      )!;
      console.log(this.communeInput);
    }    
    // this.communeInput.communeName = '';
    this.communeOutput.emit(this.communeInput);
  }

  //-- ẩn hiện input nhập xã mới
  isCommuneChange() {
    if(this.commune.length > 0) {
      this.isShowInput = !this.isShowInput;
      this.communeInput.communeName = '';
      if(!this.isShowInput) {
        this.communeInput.communeId = this.selectedCommuneId;
      }
    }
  }

  //-- get Commune by District Id
  async getComunesByDistrict(districtId: number) {
    const response: BaseResponseModel =
      await this.service.GetComunesByDistrictId(districtId);
    if (response.isSuccess) {
      this.commune = response.data;

      if(this.commune.length > 0) {
        this.selectedCommuneId = this.commune[0].communeId;
        this.isShowInput = false;
      } else {
        this.isShowInput = true;
      }
    }
  }
}
