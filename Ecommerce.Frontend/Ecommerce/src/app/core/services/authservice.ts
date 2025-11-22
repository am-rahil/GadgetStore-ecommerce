import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, RegisterRequest, UserResponse } from '../models/auth.models';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class Authservice {
  private baseUrl = `${environment.BaseUrl}/Security`;

  constructor(private http: HttpClient) { }

  // Login API call
  login(data: LoginRequest): Observable<UserResponse> {
    return this.http.post<UserResponse>(`${this.baseUrl}/login`, data);
  }

  // Register API call
  register(data: RegisterRequest): Observable<UserResponse> {
    return this.http.post<UserResponse>(`${this.baseUrl}/register`, data);
  }

  //  Save user details after login
  saveUserData(token: string, role: string, fullName: string, userId: number): void {
    localStorage.setItem('token', token);
    localStorage.setItem('role', role);
    localStorage.setItem('userName', fullName);
    localStorage.setItem('userId', userId.toString());
  }


  // Get login status
  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;

    try {
      const { exp } = jwtDecode<{ exp: number }>(token);
      // if (!exp) return true; // no exp – treat as logged in
      return Date.now() < exp * 1000;
    } catch {
      return false;
    }
  }

  hasRole(role: 'Admin' | 'Customer'): boolean {
    const current = this.getUserRole();
    return current?.toLowerCase() === role.toLowerCase();
  }


  // Get user details
  getUserName(): string | null {
    return localStorage.getItem('userName');
  }

  getUserRole(): string | null {
    return localStorage.getItem('role');
  }

  getUserId(): number {
    const id = localStorage.getItem('userId');
    return id ? Number(id) : 0;
  }

  // 🔹 Logout user
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('userName');
    localStorage.removeItem('userId');
  }


  //get all users
  getAllUsers(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/getusers`);
  }

  //delete users
  deleteUser(userId: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/delete/${userId}`)
  }

}
