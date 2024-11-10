import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ServicesService {
  private token: string = '';
  private apiUrl = 'https://localhost:7060/api/';
  constructor() {
    if (typeof window !== 'undefined') {
      this.token = localStorage.getItem('token') || '';
    }
  }

  async GetAddressById(addressId: number) {
    //https://localhost:7060/api/Address?Id=1
    const url = `${this.apiUrl}Address?Id=${addressId}`;
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`, // Nếu API yêu cầu xác thực, thêm header này
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

  async GetComunesByDistrictId(districtId: number) {
    //https://localhost:7060/api/Commune/Get By District Id?districtId=1
    const url = `${this.apiUrl}Commune/Get%20By%20District%20Id?districtId=${districtId}`;
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`, // Nếu API yêu cầu xác thực, thêm header này
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

  async GetCommunes() {
    //https://localhost:7060/api/Commune
    const url = `${this.apiUrl}Commune`;
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`, // Nếu API yêu cầu xác thực, thêm header này
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

  async GetDistrictsByProvinceId(providedId: number) {
    //https://localhost:7060/api/District/Get By Province ID?provinceID=62
    const url = `${this.apiUrl}District/Get%20By%20Province%20ID?provinceID=${providedId}`;
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`, // Nếu API yêu cầu xác thực, thêm header này
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

  async GetDistricts() {
    //https://localhost:7060/api/District
    const url = `${this.apiUrl}District`;
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`, // Nếu API yêu cầu xác thực, thêm header này
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

  async GetProvinces() {
    const url = `${this.apiUrl}Province`;
    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`, // Nếu API yêu cầu xác thực, thêm header này
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
