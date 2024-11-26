import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { ContructorSupplierRequestModerator, SupplierRequestModerator } from '../moderator.module';
import { FormsModule } from '@angular/forms';
import { ViewProductsComponent } from '../view-products/view-products.component';
import { ModeratorService } from '../moderator.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';

@Component({
  selector: 'app-supplier-detail',
  standalone: true,
  imports: [
    FormsModule,
    ViewProductsComponent
  ],
  templateUrl: './supplier-detail.component.html',
  styleUrl: './supplier-detail.component.css'
})
export class SupplierDetailComponent {
  @Input() Supplier: SupplierRequestModerator = ContructorSupplierRequestModerator();
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  isShowInsertAddress: boolean = false;

  addressString: string = ''

  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if(changes['Supplier']) {
      this.getAddressString();
    }
    console.log('Đang lấy địa chỉ string', this.addressString);
    
  }

  constructor(
    private moderatorService: ModeratorService
  ) {}

  async getAddressString() {
    const result : BaseResponseModel = await this.moderatorService.getStringAddresses(this.Supplier.addressId);
    if(result.isSuccess){
      this.addressString = result.data[0].value;
    }
  }

  sendIsClose() {
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
