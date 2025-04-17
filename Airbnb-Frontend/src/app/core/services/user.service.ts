import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {}

  getUsers() {
    return this.http.get<User[]>('https://localhost:7200/api/users/all');
  }

  deleteUser(id: string) {
    return this.http.delete<User>(`https://localhost:7200/api/users/${id}`);
  }

  // // Helper method to handle pagination
  // private getPagedUsers(count: number) {
  //   return this.getUsers().pipe(
  //     map(users => users.slice(0, count))
  //   ).toPromise();
  // }

  // getCustomersMini() {
  //   return this.getPagedUsers(5);
  // }

  // getCustomersSmall() {
  //   return this.getPagedUsers(10);
  // }

  // getCustomersMedium() {
  //   return this.getPagedUsers(50);
  // }

  // getCustomersLarge() {
  //   return this.getPagedUsers(200);
  // }

  // getCustomersXLarge() {
  //   return this.getUsers().toPromise();
  // }

  // getCustomers(params?: any) {
  //   return this.getUsers().pipe(
  //     map(users => params ? this.applyParams(users, params) : users)
  //   ).toPromise();
  // }

  // private applyParams(users: User[], params: any): User[] {
  //   // Add any filtering/sorting logic based on params here
  //   return users;
  // }

  // getCustomers(params?: any) {
    //   return this.http.get<any>('https://www.primefaces.org/data/customers', { params: params }).toPromise();
    // }

}
