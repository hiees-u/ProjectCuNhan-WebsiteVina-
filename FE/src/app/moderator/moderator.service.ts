import { Injectable } from '@angular/core';
import { BaseResponseModel } from '../shared/module/base-response/base-response.module';
import {
  CategoryRequesModerator,
  CustomerRequestModule,
  DepartmentRequestModerator,
  InsertProduct,
  ProductModerator,
  SubCategoryRequesModerator,
  SupplierRequestModerator,
  SupplierResponseModerator,
} from './moderator.module';

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

  //DELETE DEPARTMENT
  async deleteDepartment(departId: number) : Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Department?depID=${departId}`;
      //https://localhost:7060/api/Department?depID=13
      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
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

  //PUT DEPARTMENT
  async putDepartment(depar: DepartmentRequestModerator) : Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Department';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(depar),
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

  //POST EMPLOYEE
  async postEmployee(emp: {
    employeeTypeId: number;
    departmentId: number;
    accountName: string;
  }): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Employee';
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(emp),
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

  //PUT EMPLOYEE
  async putEmployee(emp: {
    accountName: string;
    employeeTypeId: number;
    departmentId: number;
    fullName: string | null;
    gender: number | null;
  }): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Employee';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(emp),
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

  //DELETE EMPLOYEE BY ACCOUNT NAME
  async deleteEmployee(accountName: string): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Employee?accountName=${accountName}`;
      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
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

  //PUT CUSTOMER
  async putCustomer(customer: {
    accountName: string;
    typeCustomerId: number;
  }): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Customer';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(customer),
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

  //GET CUSTOMER TYPE
  async getCustomerType(): Promise<BaseResponseModel> {
    const url = 'https://localhost:7060/api/CustomerType';

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

  //GET EMPLOYEE
  async getEmployee(
    employeeTypeID?: number,
    departmentID?: number,
    pageNumber: number = 1,
    pageSize: number = 42
  ): Promise<BaseResponseModel> {
    //https://localhost:7060/api/Employee?pageNumber=2&pageSize=2&departmentID=7&employeeTypeID=51
    let url = `${this.apiUrl}Employee?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    if (employeeTypeID) {
      url += `&employeeTypeID=${employeeTypeID}`;
    }
    if (departmentID) {
      url += `&departmentID=${departmentID}`;
    }

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
      console.log(data);
      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  //GET CUSTOMER
  async getCustomer(
    TypeCustomerId?: number,
    pageNumber: number = 1,
    pageSize: number = 42
  ): Promise<BaseResponseModel> {
    //https://localhost:7060/api/Customer?TypeCustomerId=3&pageNumber=1&pageSize=8
    let url = `${this.apiUrl}Customer?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    if (TypeCustomerId) {
      url += `&TypeCustomerId=${TypeCustomerId}`;
    }

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
      console.log(data);
      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  //PUT PRODUCT
  async putProduct(product: ProductModerator): Promise<BaseResponseModel> {
    const input = {
      productId: product.productId,
      productName: product.productName,
      image: product.image,
      categoryId: product.categoryId,
      supplier: product.supplier,
      subCategoryId: product.subCategoryId,
      expiryDate: product.expriryDate,
      description: product.description,
      price: product.price,
    };

    try {
      const url = 'https://localhost:7060/api/Product';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(input),
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

  //Delete Product
  async deleteProduct(productId: number): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Product?productId=${productId}`;
      //https://localhost:7060/api/Product?productId=66
      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
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

  //delete supplier
  async deleteSupplier(id: number): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Supplier?supplierId=${id}`;
      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
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

  //update supplier
  async putSupplier(
    supplier: SupplierRequestModerator
  ): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Supplier';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(supplier),
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

  //delete SubCategory
  async deleteSubCategory(id: number): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/SubCategory?subCateId=${id}`;
      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
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

  //update sub-category
  async putSubCategory(
    subCate: SubCategoryRequesModerator
  ): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/SubCategory';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(subCate),
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

  //delete Category
  async deleteCategory(id: number): Promise<BaseResponseModel> {
    try {
      const url = `https://localhost:7060/api/Category?cateId=${id}`;
      const response = await fetch(url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
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

  //update Category
  async putCategory(cate: CategoryRequesModerator): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Category';
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(cate),
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

  //
  async getStringAddresses(id: number): Promise<BaseResponseModel> {
    //https://localhost:7060/api/Address/Get string address?idAddress=1
    const url = `https://localhost:7060/api/Address/Get string address?idAddress=${id}`;
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

  //get account name
  async getAccountName(): Promise<BaseResponseModel> {
    const url = `https://localhost:7060/api/UserInfo/GET Account Name`;
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

  //post supplier
  async postSupplier(
    supplier: SupplierResponseModerator
  ): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Supplier';
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(supplier),
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

  //post sub cate
  async postDepartment(deparmentName: string): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Department';
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(deparmentName),
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

  //post sub cate
  async postSubCategory(subCateName: string): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/SubCategory';
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(subCateName),
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

  //post Cate
  async postCategory(cateName: string): Promise<BaseResponseModel> {
    try {
      const url = 'https://localhost:7060/api/Category';
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(cateName),
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

  //get Image by image name + png
  async getImage(imgName: string): Promise<string | undefined> {
    const url = `https://localhost:7060/api/File?fileName=${imgName}`;
    try {
      const response = await fetch(url);

      console.log(url);
      console.log(response);

      if (!response.ok) {
        throw new Error('Lỗi');
      }

      const blob = await response.blob();
      return URL.createObjectURL(blob);
    } catch (error) {
      console.error('Lỗi API get image: ', error);
      return undefined; // Đảm bảo trả về giá trị
    }
  }

  //get supplier
  async getSupplier(
    supplierName?: string,
    pageNumber: number = 1,
    pageSize: number = 42
  ): Promise<BaseResponseModel> {
    let url = `${this.apiUrl}Supplier/Get All?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    if (supplierName) {
      url += `&cateName=${encodeURIComponent(supplierName)}`;
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
      const data: BaseResponseModel = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  //get sub category
  async getSubCate(
    subCateName?: string,
    pageNumber: number = 1,
    pageSize: number = 42
  ): Promise<BaseResponseModel> {
    let url = `${this.apiUrl}SubCategory/Get Pagition?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    if (subCateName) {
      url += `&subCateName=${encodeURIComponent(subCateName)}`;
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
      const data: BaseResponseModel = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  //get category
  async getCate(
    cateName?: string,
    pageNumber: number = 1,
    pageSize: number = 42
  ): Promise<BaseResponseModel> {
    let url = `${this.apiUrl}Category/Get Pagition?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    if (cateName) {
      url += `&cateName=${cateName}`;
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
      const data: BaseResponseModel = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  //get all Department
  async getDeparment(
    pageNumber: number,
    pageSize: number
  ): Promise<BaseResponseModel> {
    const url = `https://localhost:7060/api/Department?pageNumber=${pageNumber}&pageSize=${pageSize}`;
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

  //post Product
  async postProduct(product: InsertProduct): Promise<BaseResponseModel> {
    const url = 'https://localhost:7060/api/Product';
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
        body: JSON.stringify(product),
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

  // Thêm các hàm gọi API cho thống kê
  async getDailySalesReport(date: Date): Promise<any> {
    try {
      if (!(date instanceof Date) || isNaN(date.getTime())) {
        throw new Error('Giá trị ngày không hợp lệ.');
      }
      const url = `${this.apiUrl}Report/Daily?InputDate=${date.toISOString()}`;
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
        },
      });
      return await response.json();
    } catch (error) {
      console.error('Error fetching daily sales report:', error);
      throw error;
    }
  }
  async getWeeklySalesReport(startDate: Date, endDate: Date): Promise<any> {
    // Đảm bảo API trả về dữ liệu theo tuần
    const url = `${this.apiUrl}Report/Weekly?StartDate=${startDate.toISOString()}&EndDate=${endDate.toISOString()}`;
    const response = await fetch(url, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${this.token}` },
    });
    return await response.json();
  }
  
  async getMonthlySalesReport(month: number, year: number): Promise<any> {
    const url = `${this.apiUrl}Report/Monthly?Month=${month}&Year=${year}`;
    const response = await fetch(url, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${this.token}` },
    });
    const data = await response.json();
    console.log('Dữ liệu thống kê theo tháng:', data); // Thêm log dữ liệu
    return data;
}

async getYearlySalesReport(year: number): Promise<any> {
    const url = `${this.apiUrl}Report/Yearly?Year=${year}`;
    const response = await fetch(url, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${this.token}` },
    });
    const data = await response.json();
    console.log('Dữ liệu thống kê theo năm:', data); // Thêm log dữ liệu
    return data;
}

  

}