// import { Injectable } from '@angular/core';

// @Injectable({
//   providedIn: 'root'
// })
// export class WarehouseEmployeeService {

//   constructor() { }
// }
import { Injectable } from '@angular/core';
// import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseResponseModule } from '../shared/module/base-response/base-response.module';

interface BaseResponseModel {
  isSuccess: boolean;
  message: string;
  data?: any;
}

interface Warehouse {
  warehouseId: number;
  warehouseName: string;
  address: string;
  fullAddress: string;
  modifiedBy: string;
  createTime: string;
  modifiedTime: string;
}

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
  }

  // Lấy token
  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
  }

  async getWarehouses(): Promise<BaseResponseModel> {
    console.log('tới đây');
    
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
      console.log(data);
      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  // GET Warehouses
  // getWarehouses(): Observable<BaseResponseModel> {
  //   const url = `${this.apiUrl}Warehouse/GetWarehouse`;
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     'Authorization': `Bearer ${this.token}`,
  //   });

  //   return this.http.get<BaseResponseModel>(url, { headers });
  // }

  // // PUT Warehouse
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
}

