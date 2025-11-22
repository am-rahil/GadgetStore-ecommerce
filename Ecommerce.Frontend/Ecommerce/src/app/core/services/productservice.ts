import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class Productservice {
  private baseUrl = `${environment.BaseUrl}/Product`;

  constructor(private http: HttpClient) { }

  //  Get all products
  getAllProducts(): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetAllProducts`);
  }

  // Get single or filtered list 
  getProductsByid(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetProductById?id=${id}`);
  }

  //  Add product (FormData handles image upload)
  addProduct(formData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}/AddProduct`, formData);
  }

  //  Delete product by ID
  deleteProduct(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/DeleteProduct?id=${id}`);
  }
  //products by category id
  getProductsBycategoryId(categoryId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/GetProductsByCategory?categoryId=${categoryId}`);
  }

  //update Product
  updateProduct(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.baseUrl}/UpdateProduct?id=${id}`, formData)
  }

  //clear gallary images
  clearGalleryImages(productId: number): Observable<any> {
  return this.http.delete(`${this.baseUrl}/ClearGalleryImages?productId=${productId}`);
}

}
