import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Listing } from '../models/Listing';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // Signal to store listings
  listings = signal<Listing[] | null>(null);
  isLoading = signal(false);
  hasError = signal(false);

  constructor(private http : HttpClient) { }

  // Function to call the backend API
  // loadListings() {
  //   this.isLoading.set(true);
  //   this.hasError.set(false);

  //   this.http.get<Listing[]>('https://localhost:7200/api/Listings?queryParams%5BadditionalProp1%5D=string&queryParams%5BadditionalProp2%5D=string&queryParams%5BadditionalProp3%5D=string')
  //     .subscribe({
  //       next: (data) => {
  //         this.listings.set(data);
  //         this.isLoading.set(false);
  //       },
  //       error: () => {
  //         this.hasError.set(true);
  //         this.isLoading.set(false);
  //       }
  //     });
  // }

}
