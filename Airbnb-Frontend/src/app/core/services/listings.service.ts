import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Listing } from '../models/Listing';

@Injectable({
  providedIn: 'root'
})
export class ListingsService {
  private apiUrl = 'https://localhost:5001/api';
  constructor(private http : HttpClient) { }

  getListings() {
    return this.http.get<Listing[]>(`${this.apiUrl}/Listings`);
  }

  getListingById(id: string) {
    return this.http.get<Listing>(`${this.apiUrl}/Listings/${id}`);
  }

  deleteListing(id: string) {
    return this.http.delete<Listing>(`${this.apiUrl}/Listings/${id}`);
  }

  getDraftListing(id: string) {
    return this.http.get<Listing>(`${this.apiUrl}/Listings/drafts/${id}`);
  }

  updateDraftListing(id: string, formData: any) {
    return this.http.put<Listing>(`${this.apiUrl}/Listings/drafts/${id}`, formData);
  }

  uploadListingPhoto(listingId: string, file: File) {
    const formData = new FormData();
    formData.append('photo', file);
    return this.http.post<{ photoUrl: string }>(`${this.apiUrl}/Listings/${listingId}/photos`, formData);
  }

  publishListing(id: string){
    return this.http.post<Listing>(`${this.apiUrl}/Listings/drafts/${id}/publish`, {});
  }

}
