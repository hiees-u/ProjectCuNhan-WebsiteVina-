import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TransportStaffEmployeeService {

  private token: string = '';
  private apiUrl = 'https://localhost:7060/api/Order/getOrdersByTS';
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

  async fetchOrders(): Promise<any> {
    try {
        const url = `${this.apiUrl}?pageNumber=1&pageSize=99`;
        console.log(url);
        

        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${this.token}`,
            }
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(`LỖI ở xuất hóa đơn: ${errorData.message}`);
        }

        const responseData = await response.json();
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
