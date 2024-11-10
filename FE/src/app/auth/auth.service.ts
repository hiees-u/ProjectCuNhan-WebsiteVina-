import { Injectable } from '@angular/core';
import { Login } from '../shared/module/login/login.module';
import { BaseResponseModel } from '../shared/module/base-response/base-response.module';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7060/api';

  async login(user: Login): Promise<BaseResponseModel> {
    try {
      const response = await fetch(`${this.apiUrl}/User/Login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(user),
      });

      if (!response.ok) {
        const errorResponse = await response.json();
        console.error('Error:', errorResponse);
        throw new Error(
          `HTTP error! status: ${response.status}, message: ${errorResponse.message}`
        );
      }

      return await response.json();
    } catch (error) {
      console.error('Error:', error);
      throw error;
    }
  }

  async getRole(): Promise<BaseResponseModel> {
    // Lấy token từ localStorage
    const token = localStorage.getItem('token');

    try {
      const response = await fetch(`${this.apiUrl}/User/GetRole`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`, // Thêm token vào tiêu đề
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
