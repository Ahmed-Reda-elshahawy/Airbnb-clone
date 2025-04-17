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
  getCurrentUser() {
    return this.http.get<User>(`${this.apiUrl}/users/me`);
  }
}
