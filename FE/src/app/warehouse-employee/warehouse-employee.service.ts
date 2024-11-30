import { Injectable } from '@angular/core';
import {
  BaseResponseModel,
  BaseResponseModule,
} from '../shared/module/base-response/base-response.module';
import { Warehouse } from './warehouse-employee.module';
@Injectable({
  providedIn: 'root',
})
export class WarehouseEmployeeService {
  private token: string = '';
  private apiUrl = 'https://localhost:7060/api/';
  
  constructor() {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }

    console.log = console.log || function (...args: any[]) { /* Do nothing */ };
    console.error = console.error || function (...args: any[]) { /* Do nothing */ };
  }

  // Lấy token
  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
  }
  
  // async getWarehouses(): Promise<BaseResponseModel> {
  //   const url = `${this.apiUrl}Warehouse/GetWarehouse`;
  //   try {
  //     const response = await fetch(url, {
  //       method: 'GET',
  //       headers: {
  //         'Content-Type': 'application/json',
  //         Authorization: `Bearer ${this.token}`,
  //       },
  //     });
  //     if (!response.ok) {
  //       throw new Error(`HTTP error! status: ${response.status}`);
  //     }
  //     const data: BaseResponseModel = await response.json();
  //     console.log(data);
  //     return data;
  //   } catch (error) {
  //     console.error('Error:', error);
  //     throw error;
  //   }
  // }
  async getWarehouses(): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}Warehouse/GetWarehouse`;
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data: BaseResponseModel = await response.json();

      // Đảm bảo console hoạt động trước khi log
      if (console && typeof console.log === 'function') {
        console.log('Dữ liệu lấy được:', data);
      }

      return data;
    } catch (error) {
      // Đảm bảo console.error hoạt động
      if (console && typeof console.error === 'function') {
        console.error('Lỗi khi gọi API:', error);
      }

      throw error; // Tiếp tục ném lỗi để xử lý tại thành phần gọi hàm
    }
  }
  //Delete Warehouse
  // async deleteWarehouse(warehousreId: number): Promise<BaseResponseModel> {
  //   try {
  //     const url = `https://localhost:7060/api/Product?productId=${productId}`;
  //     //https://localhost:7060/api/Product?productId=66
  //     const response = await fetch(url, {
  //       method: 'DELETE',
  //       headers: {
  //         'Content-Type': 'application/json',
  //         Authorization: `Bearer ${this.token}`,
  //       },
  //     });
  //     const data: BaseResponseModel = await response.json();
  //     console.log(data);
  //     return data;
  //   } catch (error) {
  //     console.log('Lỗi: ', error);
  //     return {
  //       isSuccess: false,
  //       message: 'Lỗi ròi mài ơi',
  //     };
  //   }
  // }
}


  // PUT Warehouse

  // async putWarehouse(warehouse: Warehouse): Promise<BaseResponseModel> {
  //   const url = `${this.apiUrl}Warehouse`;
  //   const headers = {
  //     'Content-Type': 'application/json',
  //     'Authorization': `Bearer ${this.token}`,
  //   };
  //   const body = JSON.stringify(warehouse);

  //   try {
  //     const response = await fetch(url, {
  //       method: 'PUT',
  //       headers: headers,
  //       body: body,
  //     });
  //     const data: BaseResponseModel = await response.json();
  //     console.log(data);
  //     return data;
  //   } catch (error) {
  //     console.log('Lỗi: ', error);
  //     return {
  //       isSuccess: false,
  //       message: 'Lỗi ròi mài ơi',
  //     };
  //   }
  // }

  // // DELETE Warehouse

  // async deleteWarehouse(warehouseId: number): Promise<BaseResponseModel> {
  //   const url = `${this.apiUrl}Warehouse?warehouseId=${warehouseId}`;
  //   const headers = {
  //     'Content-Type': 'application/json',
  //     'Authorization': `Bearer ${this.token}`,
  //   };

  //   try {
  //     const response = await fetch(url, {
  //       method: 'DELETE',
  //       headers: headers,
  //     });
  //     const data: BaseResponseModel = await response.json();
  //     console.log(data);
  //     return data;
  //   } catch (error) {
  //     console.log('Lỗi: ', error);
  //     return {
  //       isSuccess: false,
  //       message: 'Lỗi ròi mài ơi',
  //     };
  //   }
  // }
//}

