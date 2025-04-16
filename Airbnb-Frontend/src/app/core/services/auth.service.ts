import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { loginUser } from './../models/loginUser';
import { RegisterUser } from '../models/registerUser';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = 'https://localhost:5001/api';
  constructor(private http: HttpClient) { }

  login(user:loginUser) {
    return this.http.post<loginUser>(`${this.apiUrl}/Authentication/login`, user);
  }
  register(user:RegisterUser){
    return this.http.post<RegisterUser>(`${this.apiUrl}/Authentication/register`, user);
  }
}
