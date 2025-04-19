import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Listing } from '../models/Listing';
import { catchError, map, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ListingsService {
  apiUrl = 'https://localhost:7200/api';
  hostListingsSignal = signal<Listing[]>([]);
  hostDraftsSignal = signal<Listing[]>([]);
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

  getListingsByHostId(hostId: string) {
    return this.http.get<Listing[]>(`${this.apiUrl}/Listings/host/${hostId}`).pipe(
      tap(listings => {
        this.hostListingsSignal.set(listings);
      }),
      catchError(error => {
        return of(null);
      })
    )
  }

  getEmptyListingsByHostId(hostId: string) {
    return this.http.get<Listing[]>(`${this.apiUrl}/Listings/host/${hostId}`).pipe(
      map(listings => {
        const drafts = listings.filter(listing => listing.title === '');
        this.hostDraftsSignal.set(drafts);
        return drafts;
      }),
      catchError(error => {
        this.hostDraftsSignal.set([]);
        return of([]);
      })
    )
  }

  createEmptyListing() {
    return this.http.post<Listing>(`${this.apiUrl}/listings/empty`, null).pipe(
      tap(listing => {
        this.hostDraftsSignal.update(drafts => [...drafts, listing]);
      }),
      catchError(error => {
        return of(null);
      })
    )
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
