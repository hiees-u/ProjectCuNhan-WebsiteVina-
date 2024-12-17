import { Injectable } from '@angular/core';
import { BaseResponseModel } from '../shared/module/base-response/base-response.module';

@Injectable({
  providedIn: 'root',
})
export class TransportStaffEmployeeService {
  private token: string = '';
  // private apiUrl = 'https://localhost:7060/api/Order/getOrdersByTS';
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

  async giaoHang(orderId: number): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Order/GiaoHang?IdOrder=${orderId}`;

      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(`LỖI: ${errorData.message}`);
      }

      const responseData = await response.json();
      console.log(response);

      return responseData;
    } catch (error) {
      console.log(error);
      return {
        isSuccess: false,
        message: 'Lỗi..!',
      };
    }
  }

  async fetchOrders(): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Order/getOrdersByTS?pageNumber=1&pageSize=99';
      console.log(url);

      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(`LỖI: ${errorData.message}`);
      }

      const responseData = await response.json();
      console.log(response);

      return responseData;
    } catch (error) {
      console.log(error);
      return {
        isSuccess: false,
        message: 'Lỗi..!',
      };
    }
  }
}
