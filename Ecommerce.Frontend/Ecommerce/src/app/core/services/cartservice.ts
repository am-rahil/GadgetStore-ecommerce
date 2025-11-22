import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class Cartservice {
  private baseUrl = `${environment.BaseUrl}/CartItem`;
  constructor(private http: HttpClient) { }


 //  Get cart by user
  getUserCart(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetUserCart?userId=${userId}`);
  }

  //  Add or update item
  addOrUpdateCartItem(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/AddOrUpdateCartItem`, data);
  }

  //  Remove specific cart item
  removeCartItem(cartId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/RemoveCartItem?cartId=${cartId}`);
  }

  // Clear entire cart
  clearCart(userId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/ClearCart?userId=${userId}`);
  }

  //create order
  createOrder(data: any) {
  return this.http.post(`${environment.BaseUrl}/Order/CreateOrder`, data);
}
}
