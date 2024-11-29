import { Injectable } from '@angular/core';
import { BaseResponseModel } from '../shared/module/base-response/base-response.module';
import { CartItem, CartResponse } from '../shared/module/cart/cart.module';
import { UserInfoRequestModel } from '../shared/module/user-info/user-info.module';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { AddressRequest } from '../shared/module/address/address.module';
import { OrderRequestModule } from '../shared/module/order/order.module';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private data = new BehaviorSubject<CartItem[]>([]); //test
  currentData = this.data.asObservable();

  sendData(data: CartItem[]) {
    this.data.next(data);
  }

  private token: string = '';
  private apiUrl = 'https://localhost:7060/api/';
  constructor() {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
  }
  
  async createPaymentMomo(order: OrderRequestModule): Promise<any> {
    const url = `${this.apiUrl}Payment/CreatePaymentUrl`;
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(order),
      });
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      console.log('MOMO Payment Response:', data); // Log phản hồi từ server
      return data;
    } catch (error) {
      console.error('Error creating MOMO payment URL:', error);
      throw error;
    }
  }
  
  //lấy token
  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
  }

  //Lấy danh sách đơn hàng:
  async getOrder(oredrState: number): Promise<BaseResponseModel> {
    //https://localhost:7060/api/Order?orderState=0
    const url = `${this.apiUrl}Order?orderState=${oredrState}`;

    return fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token}`,
      }
    }).then((response) => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json() as Promise<BaseResponseModel>;
    });
  }

  //Delete Order detail
  async deleteOrder(orderId: Number, priceHistoryId: number): Promise<BaseResponseModel> {
    //https://localhost:7060/api/Order?OrderId=12&PriceHistory=1
    const url = `${this.apiUrl}Order?OrderId=${orderId}&PriceHistory=${priceHistoryId}`;
    return await fetch(url, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token}`,
      }
    }).then((response) => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json() as Promise<BaseResponseModel>;
    });
  }

  //post order
  async postOrder(order: OrderRequestModule) : Promise<BaseResponseModel> {
    //https://localhost:7060/api/Order
    const url = `${this.apiUrl}Order`;

    console.log('log order ở service');
    console.log(order);
    
    

    return fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token}`,
      },
      body: JSON.stringify(order),
    }).then((response) => {
      if (!response.ok) {
        throw new Error('Lỗi ở Post Order');
      }
      return response.json() as Promise<BaseResponseModel>;
    });
  }

  //post address
  async postAddress(address: AddressRequest) : Promise<BaseResponseModel> {
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
  async getStringAddresses(): Promise<BaseResponseModel> {
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

  async putUserInfo(uf: UserInfoRequestModel): Promise<BaseResponseModel> {
    //https://localhost:7060/api/UserInfo
    const url = `${this.apiUrl}UserInfo`;
    const option = {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token}`,
      },
      body: JSON.stringify(uf),
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

  async getUserInfo(): Promise<BaseResponseModel> {
    //https://localhost:7060/api/UserInfo
    const url = `${this.apiUrl}UserInfo`;
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }

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
      return await response.json();
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async updateCart(cart: CartResponse): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}Cart`;

    const response = await fetch(url, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.token}`,
      },
      body: JSON.stringify(cart),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.json();
  }

  async deleteCart(productId: number): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}Cart`;
    // const token = localStorage.getItem('token');

    try {
      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(productId),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data: BaseResponseModel = await response.json();

      return data;
    } catch (error) {
      console.error('ERROR:', error);
      throw error;
    }
  }

  async postCart(response: CartResponse): Promise<BaseResponseModel> {
    //https://localhost:7060/api/Cart
    const url = `${this.apiUrl}Cart`;
    // const token = localStorage.getItem('token');
    const body = { productId: response.productId, quantity: response.quantity };
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(body),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data: BaseResponseModel = await response.json();

      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getCart(): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}Cart`;
    // const token = localStorage.getItem('token');
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

      // const data: CartResponse[] = await response.json();

      return await response.json();
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getSupplierByID(supplierId: number): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}Supplier/Get Supplier By Id?id=${supplierId}`;
    // const token = localStorage.getItem('token');
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

      return (await response.json()) as BaseResponseModel;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getCateByProductID(productId: number): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}Category/Get By Id Product?productID=${productId}`;
    // const token = localStorage.getItem('token');

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

      return (await response.json()) as BaseResponseModel;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getSubCateByProductID(productId: number): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}SubCategory/Get By Product Id?productID=${productId}`;
    // const token = localStorage.getItem('token');

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

      return (await response.json()) as BaseResponseModel;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getTop10SubCate(): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}SubCategory/Get 10 Sub Category`;
    // const token = localStorage.getItem('token');

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

      return (await response.json()) as BaseResponseModel;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getTop10Cate(): Promise<BaseResponseModel> {
    const url = `${this.apiUrl}Category/Get 10 Category`;
    // const token = localStorage.getItem('token');

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

      return (await response.json()) as BaseResponseModel;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getProducts(
    productId: number | null,
    cateId: number | null,
    subCateId: number | null,
    supplierId: number | null,
    productName: string | null,
    pageNumber: number | 1,
    pageSize: number | 1,
    sortByName: number | 0,
    sortByPrice: number | 0
  ): Promise<BaseResponseModel> {
    const params: { [key: string]: any } = {
      productId,
      cateId,
      subCateId,
      supplierId,
      productName,
      pageNumber,
      pageSize,
      sortByName,
      sortByPrice,
    };
    console.log(params);
    
    const queryString = Object.keys(params)
      .filter((key) => params[key] !== null && params[key] !== undefined)
      .map(
        (key) => `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`
      )
      .join('&');
    const url = `${this.apiUrl}Product?${queryString}`;
    console.log(url);

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
      return await response.json();
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }
}
