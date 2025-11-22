import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Orderservice {

  private baseUrl = `${environment.BaseUrl}/Order`;
  constructor(private http: HttpClient) { }


  createOrder(orderData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/CreateOrder`, orderData);
  }

  getUserOrders(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetAllOrders?userId=${userId}`);
  }

  getOrderById(orderId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetOrderById?id=${orderId}`);
  }


  updateOrderStatus(orderId: number, status: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/UpdateOrderStatus?id=${orderId}&status=${status}`, {}, { responseType: 'text' });
  }


  getAllOrders(): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetAllOrders`);
  }

  cancelOrder(orderId: number): Observable<any> {
    return this.updateOrderStatus(orderId, 'Cancelled');
  }

}
