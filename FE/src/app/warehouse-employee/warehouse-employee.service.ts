import { Injectable } from '@angular/core';
import {
  BaseResponseModel,
  BaseResponseModule,
} from '../shared/module/base-response/base-response.module';
import { Warehouse, PostWareHouseRequestWarehouseEmployee, ExportWarehouseRequest, ImportWarehouseRequest } from './warehouse-employee.module';
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
  }

  // Lấy token
  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
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
        console.log('Dữ liệu lấy được từ service(API):', data);
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
      return data;
    } catch (error) {
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
      return data;
    } catch (error) {
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
    const url = `https://localhost:7060/api/Address/Get string address?idAddress=${addressId}`;
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
      return data;
    } catch (error) {
      console.error('Lỗi: ', error);
      return {
        isSuccess: false,
        message: 'Lỗi ròi mài ơi',
      };
    }
  }
  async getWarehousesByName(warehouseName: string): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Warehouse/GetInForWarehouseByName/${encodeURIComponent(warehouseName)}`;
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });

      if (!response.ok) {
        return {
          isSuccess: false,
          message: `Lỗi lấy danh sách kho: ${response.statusText}`,
        };
      }

      const data: BaseResponseModel = await response.json();
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: 'Lỗi kết nối đến máy chủ',
      };
    }
  }


  //===========Export Warehouse=============================
  async exportGoodsByOrder(request: ExportWarehouseRequest): Promise<BaseResponseModel> {
    const url = `https://localhost:7060/api/DeliveryNote/InsertDeliveryNote`;

    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(request),
      });

      const data: BaseResponseModel = await response.json();
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: 'Có lỗi xảy ra trong quá trình xuất phiếu nhập kho',
      };
    }
  }

  async getOrderIds(): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}DeliveryNote/GetOrderIDs`;
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
        console.log('Dữ liệu lấy được service(API) OrderIDs :', data);
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

  async getOrderDetails(orderId: number): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}DeliveryNote/GetOrderDetail?orderID=${orderId}`;
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
      console.error('Data OrderDetail', data);
      return data;
    } catch (error) {
      console.error('Error fetching order details:', error);
      throw error;
    }
  }

  //===========Import Warehouse=============================
  async importWarehouseByPurchaseOrder(request: ImportWarehouseRequest): Promise<BaseResponseModel> {
    const url = `https://localhost:7060/api/WarehouseReceipt/InsertWarehouseReceipt`;

    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(request),
      });

      const data: BaseResponseModel = await response.json();
      return data;
    } catch (error) {
      return {
        isSuccess: false,
        message: 'Có lỗi xảy ra trong quá trình xuất phiếu nhập kho',
      };
    }
  }

  async GetUndeliveredPurchaseOrders(): Promise<BaseResponseModel> {
    const url = `https://localhost:7060/api/WarehouseReceipt/GetUndeliveredPurchaseOrders`;
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
        console.log('Dữ liệu lấy được service(API) GetUndeliveredPurchaseOrders :', data);
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

  async GetPurchaseOrderDetails(purchaseOrderID: number): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}WarehouseReceipt/GetPurchaseOrderDetails?purchaseOrderID=${purchaseOrderID}`;
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
      console.error('Data GetPurchaseOrderDetails', data);
      return data;
    } catch (error) {
      console.error('Error fetching order details:', error);
      throw error;
    }
  }

  async getProductsExpriryDate(): Promise<BaseResponseModel> {
    const url = `https://localhost:7060/api/Cell/GetProductsExpriryDate`;
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
        console.log('Dữ liệu lấy được từ service(API):', data);
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
}

