import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { OrderApproverService } from '../order-approver.service';
import { OrderDetailOA } from '../order-approver.module';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';

@Component({
  selector: 'app-view-order-detail',
  imports: [
    CommonModule,
    CustomCurrencyPipe
  ],
  templateUrl: './view-order-detail.component.html',
  styleUrl: './view-order-detail.component.css'
})
export class ViewOrderDetailComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() OrderID: number = 0;

  OrderDetail: OrderDetailOA[] = [];

  constructor(private serviceOrderApprover: OrderApproverService) {}

  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if(changes['OrderID']) {
      this.getOrderDetail(this.OrderID);
      console.log('change');
      
    }
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.getOrderDetail(this.OrderID);
    console.log('Laafn nuawx');
    
  }

  async getOrderDetail(oid: number) {
    const res = await this.serviceOrderApprover.getOrderDetailApprover(oid);
    if(res.isSuccess) {
      this.OrderDetail = res.data;
      console.log(this.OrderDetail);
      
    }
  }

  sendIsClose() {
    // this.onDelete();
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
