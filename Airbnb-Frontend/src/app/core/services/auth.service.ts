import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { loginUser } from './../models/loginUser';
import { RegisterUser } from '../models/registerUser';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = 'https://localhost:5001/api';
  currentUserSignal = signal<undefined | null | User>(undefined);
  constructor(private http: HttpClient) { }

  login(user: loginUser) {
    return this.http.post<unknown>(`${this.apiUrl}/Authentication/login`, user);
  }
  register(user: RegisterUser) {
    return this.http.post<RegisterUser>(`${this.apiUrl}/Authentication/register`, user);
  }
}
