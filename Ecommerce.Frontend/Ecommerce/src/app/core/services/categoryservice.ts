import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class Categoryservice {
  private baseUrl = `${environment.BaseUrl}/Category`;

  constructor(private http: HttpClient) { }
  getallCategories(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/GetAllCategories`)
  }

  addCategory(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/AddCategory`, data);
  }

  updateCategory(id: number, data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/UpdateCategory?id=${id}`, data);
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/DeleteCategory?id=${id}`);
  }

  getCategoryById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetCategoryById?id=${id}`);
  }
}
