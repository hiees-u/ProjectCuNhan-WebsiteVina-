import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CustomerService } from '../customer.service';  // Giả sử service để giao tiếp backend

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {
  paymentData: any = {};
  paymentSuccess: boolean = false;

  constructor(private route: ActivatedRoute, private customerService: CustomerService) { }

  ngOnInit(): void {
    // Lấy các tham số từ URL
    this.route.queryParams.subscribe(params => {
      console.log('Thông tin trả về từ VNPay:', params);

      this.paymentData = {
        paymentMethod: params['payment_method'],
        orderId: params['order_id'],  // Đảm bảo lấy đúng order_id
        amount: params['vnp_Amount'],
        transactionNo: params['vnp_TransactionNo'],
        responseCode: params['vnp_ResponseCode'],
        secureHash: params['vnp_SecureHash'],
        txnRef: params['vnp_TxnRef']
      };

      // Kiểm tra xem order_id có giá trị không
      if (!this.paymentData.orderId) {
        console.error('Lỗi: Không có order_id trong URL!');
      }

      // Gọi backend để kiểm tra và cập nhật trạng thái thanh toán
      this.processPayment();
    });
  }

  // Gọi backend để xử lý thông tin thanh toán
  processPayment() {
    if (this.paymentData.paymentMethod === 'vnpay') {
      this.customerService.processVnpayCallback(this.paymentData).subscribe(response => {
        if (response.success) {
          this.paymentSuccess = true;
        } else {
          this.paymentSuccess = false;
        }
      }, error => {
        console.error('Lỗi khi xử lý thanh toán:', error);
        this.paymentSuccess = false;
      });
    }
  }
}
