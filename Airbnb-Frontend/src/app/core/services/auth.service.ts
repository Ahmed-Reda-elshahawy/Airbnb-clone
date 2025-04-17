import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { loginUser } from './../models/loginUser';
import { RegisterUser } from '../models/registerUser';
import { ResponseUser } from '../models/responseUser';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = 'https://localhost:5001/api';
  currentUserSignal = signal<undefined | null | ResponseUser | User>(undefined);
  constructor(private http: HttpClient) { }

  login(user: loginUser) {
    return this.http.post<any>(`${this.apiUrl}/Authentication/login`, user);
  }

  register(user: RegisterUser) {
    return this.http.post<RegisterUser>(`${this.apiUrl}/Authentication/register`, user);
  }

  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.currentUserSignal.set(null);
  }

  getCurrentUser() {
    return this.http.get<User>(`${this.apiUrl}/users/me`);
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  getAccessTokenData(): any {
    const token = this.getAccessToken();
    if (!token) return null;

    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (error) {
      return null;
    }
  }

  getAccessTokenClaim(claimName: string): any {
    const tokenData = this.getAccessTokenData();
    return tokenData ? tokenData[claimName] : null;
  }

  isTokenExpired() {
    const tokenData = this.getAccessTokenData();
    if (!tokenData || !tokenData.exp) return true;

    // exp is in seconds since epoch, Date.now() is in milliseconds
    return (tokenData.exp * 1000) < Date.now();
  }

}
