import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';
import { OrderApproverService } from '../order-approver.service';
import { OrderDetailOA } from '../order-approver.module';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../shared/module/customCurrency';
import { CustomerService } from '../../customer/customer.service';
import { BaseResponseModel } from '../../shared/module/base-response/base-response.module';
import {
  ConstructerNotification,
  Notification,
} from '../../shared/module/notification/notification.module';
import { NotificationComponent } from '../../shared/item/notification/notification.component';

@Component({
  selector: 'app-view-order-detail',
  imports: [CommonModule, CustomCurrencyPipe, NotificationComponent],
  templateUrl: './view-order-detail.component.html',
  styleUrl: './view-order-detail.component.css',
})
export class ViewOrderDetailComponent {
  @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() OrderID: number = 0;
  @Input() Address: string = '';
  @Input() Name: string = '';

  OrderDetail: OrderDetailOA[] = [];

  total: number = 0;

  //-------------
  trigger: any;
  dataNotification: Notification = ConstructerNotification();

  constructor(
    private serviceOrderApprover: OrderApproverService,
    private serviceCustomer: CustomerService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    if (changes['OrderID']) {
      this.getOrderDetail(this.OrderID);
      console.log('change', this.OrderID);
      console.log(this.Address);
      console.log(this.Name);
      console.log(this.OrderDetail);
    }
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.getOrderDetail(this.OrderID);
    console.log('Laafn nuawx');
  }

  async handleDeleteOrderDetail(priceHistoryId: number) {
    console.log(priceHistoryId);
    console.log(this.OrderID);
    this.trigger = new Date();
    const response: BaseResponseModel = await this.serviceCustomer.deleteOrder(
      this.OrderID!,
      priceHistoryId!
    );
    this.dataNotification.messages = response.message ? response.message : '';
    if (response.isSuccess) {
      this.dataNotification.status = 'success';
      this.dataNotification.messages = 'Xóa chi tiết đơn hàng thành công..!';
      this.getOrderDetail(this.OrderID);
    } else {
      this.dataNotification.status = 'error';
      this.dataNotification.messages = 'Vui lòng thử lại...!';
    }
  }

  async handleGenerateInvoice() {
    console.log(this.Address + ' - ' + this.Name + ' - ' + this.OrderID);
    const order = {
      orderId: this.OrderID,
      customerName: this.Name,
      customerAddress: this.Address,
    };
    try {
      const response = await this.serviceOrderApprover.GenerateInvoice(order);
      if (response.headers.get('content-type') === 'application/pdf') {
        const blob = await response.blob();
        const contentDisposition = response.headers.get('Content-Disposition');
        let fileName = 'invoice.pdf';
        if (contentDisposition) {
          const match = contentDisposition.match(/filename="(.+?)"/);
          if (match) {
            fileName = match[1];
          }
        }
        this.downloadFile(blob, fileName);
      } else {
        const result = await response.json();
        if (!result.IsSuccess) {
          console.error('Error generating invoice:', result.Message);
        }
      }
    } catch (error) {
      console.error('Error generating invoice:', error);
    }
  }

  downloadFile(data: Blob, fileName: string) {
    const blob = new Blob([data], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
  }

  async getOrderDetail(oid: number) {
    const res = await this.serviceOrderApprover.getOrderDetailApprover(oid);
    if (res.isSuccess) {
      this.OrderDetail = res.data;
      console.log(this.OrderDetail);
      this.OrderDetail.forEach((x) => {
        this.total += x.gia * x.soLuongMua;
      });
    }
  }

  sendIsClose() {
    // this.onDelete();
    console.log('THOÁT', new Date());
    this.isClose.emit(true);
  }
}
