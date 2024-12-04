import { Injectable } from '@angular/core';
import {
  BaseResponseModel,
  BaseResponseModule,
} from '../shared/module/base-response/base-response.module';
import { Warehouse, PostWareHouseRequestWarehouseEmployee } from './warehouse-employee.module';
import { AddressRequest } from '../shared/module/address/address.module';
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

    // console.log = console.log || function (...args: any[]) { /* Do nothing */ };
    // console.error = console.error || function (...args: any[]) { /* Do nothing */ };
    // console.log('aaaaaa');

  }

  // Lấy token
  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }

    console.log('aaaaaa');
  }

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
  async deleteWarehouse(warehousreId: number): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Warehouse/DeleteWareHouse/${warehousreId}`;
      //https://localhost:7060/api/Warehouse/DeleteWareHouse/4

      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });
      const data: BaseResponseModel = await response.json();
      // console.log(data);
      return data;
    } catch (error) {
      // console.log('Lỗi: ', error);
      return {
        isSuccess: false,
        message: 'Lỗi xóa Warehouse',
      };
    }
  }

  //post sub cate
  async postWarehouse(warehouse: {
    warehouseName: string;
    addressId: number;
  }): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Warehouse/PostWarehouse';
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(warehouse),
      });
      const data: BaseResponseModel = await response.json();
      // console.log(data);
      return data;
    } catch (error) {
      // console.log('Lỗi: ', error);
      return {
        isSuccess: false,
        message: 'Lỗi ròi mài ơi',
      };
    }
  }

  //post address
  async postAddress(address: AddressRequest): Promise<BaseResponseModel> {
    // https://localhost:7060/api/Address
    const url = `${this.apiUrl}Address`;

    return fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token}`,
      },
      body: JSON.stringify(address),
    }).then((response) => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json() as Promise<BaseResponseModel>;
    });
  }

  //lấy danh sách address của khách hàng
  async getStringAddresses(addressId: number): Promise<BaseResponseModel> {
    //https://localhost:7060/api/Address/Get string address
    const url = `${this.apiUrl}Address/Get string address`;
    const option = {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token}`,
      },
    };

    try {
      const response = await fetch(url, option);

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  // PUT Warehouse

  async putWarehouse(
    warehouse: PostWareHouseRequestWarehouseEmployee
  ): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Warehouse/PutWarehouse';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(warehouse),
      });
      const data: BaseResponseModel = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.log('Lỗi: ', error);
      return {
        isSuccess: false,
        message: 'Lỗi ròi mài ơi',
      };
    }
  }

  // async putWarehouse(warehouse: Warehouse): Promise<BaseResponseModel> {
  //   const input = {
  //     warehouseId: warehouse.warehouseId,
  //     warehousename: warehouse.warehouseName,
  //     addressid: warehouse.addressId
  //   }

  //   try {
  //     const url = 'https://localhost:7060/api/Warehouse/PutWarehouse';
  //     const response = await fetch(url, {
  //       method: 'PUT',
  //       headers: {
  //         'Content-Type': 'application/json',
  //         Authorization: `Bearer ${this.token}`,
  //       },
  //       body: JSON.stringify(input),
  //     });
  //     const data: BaseResponseModel = await response.json();
  //     // console.log(data);
  //     return data;
  //   } catch (error) {
  //     // console.log('Lỗi: ', error);
  //     return {
  //       isSuccess: false,
  //       message: 'Lỗi ròi mài ơi',
  //     };
  //   }
  // }
}

