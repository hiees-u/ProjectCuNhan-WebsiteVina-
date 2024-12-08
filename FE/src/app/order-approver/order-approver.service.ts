import { Injectable } from '@angular/core';
import { BaseResponseModel } from '../shared/module/base-response/base-response.module';

@Injectable({
  providedIn: 'root',
})
export class OrderApproverService {
  private token: string = '';
  // private apiUrl = 'https://localhost:7060/api/';
  constructor() {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
  }

  //lấy token
  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
  }

  async GenerateInvoice(order: any) : Promise<any> {
    try {
      const url = `https://localhost:7060/api/Order/GenerateInvoice`;

      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(order)
      });

      if(!response.ok) {
        throw new Error('LỖI ở xuất hóa đơn');
      }

      return response;

    } catch(error) {
      console.log(error);
      return {
        isSuccess: false,
        message: 'Lỗi..!'
      }
    }
  }

  async getOrderDetailApprover(Oid: number): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Order/Get Order Detail by OrderApprover?Oid=${Oid}`;

      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });

      const data: BaseResponseModel = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.log(error);
      return {
        isSuccess: false,
        message: 'Lỗi ròi mài ơi',
      };
    }
  }

  async getOrderApprover(): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Order/Get by OrderApprover';

      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });

      const data: BaseResponseModel = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.log(error);
      return {
        isSuccess: false,
        message: 'Lỗi ròi mài ơi',
      };
    }
  }
}
