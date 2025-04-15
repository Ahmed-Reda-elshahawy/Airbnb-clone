import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Listing } from '../models/Listing';

@Injectable({
  providedIn: 'root'
})
export class ListingsService {
  constructor(private http : HttpClient) { }

  getListings() {
    return this.http.get<Listing[]>('https://localhost:7200/api/Listings');
  }

  getListingById(id: string) {
    return this.http.get<Listing>(`https://localhost:7200/api/Listings/${id}`);
  }

  deleteListing(id: string) {
    return this.http.delete<Listing>(`https://localhost:7200/api/Listings/${id}`);
  }
}
