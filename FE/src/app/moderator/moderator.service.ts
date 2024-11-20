import { Injectable } from '@angular/core';
import { BaseResponseModel } from '../shared/module/base-response/base-response.module';
import { InsertProduct } from './moderator.module';

@Injectable({
  providedIn: 'root',
})
export class ModeratorService {
  private token: string = '';
  private apiUrl = 'https://localhost:7060/api/';
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

  //post Product
  async postProduct(product: InsertProduct): Promise<BaseResponseModel> {
    const url = 'https://localhost:7060/api/Product'
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(product)
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

  //file ảnh
  async UploadFile(file: File): Promise<BaseResponseModel> {
    const url = 'https://localhost:7060/api/File/upload';
    const formData = new FormData();
    formData.append('file', file);
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: { Authorization: `Bearer ${this.token}` },
        body: formData,
      });
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      return data as BaseResponseModel;
    } catch (error) {
      console.error('File upload failed:', error);
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
    const queryString = Object.keys(params)
      .filter((key) => params[key] !== null && params[key] !== undefined)
      .map(
        (key) => `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`
      )
      .join('&');
    const url = `${this.apiUrl}Product/Get%20Products%20By%20Moderator?${queryString}`;
    //https://localhost:7060/api/Product/Get Products By Moderator?productId=1&cateId=1&subCateId=1&supplierId=1&productName=a&pageNumber=1&pageSize=10&sortByName=1&sortByPrice=1
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
      const data: BaseResponseModel = await response.json();
      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }
}
