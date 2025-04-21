import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Listing } from '../models/Listing';
import { catchError, map, of, tap } from 'rxjs';
import { PropertyType } from '../models/PropertyType';
import { RoomType } from '../models/RoomType';
import { Amenity } from '../models/Amenity';
import { NewListing } from '../models/NewListing';

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

  getPropertyTypes() {
    return this.http.get<PropertyType[]>(`${this.apiUrl}/PropertyTypes`);
  }

  getAmenities() {
    return this.http.get<Amenity[]>(`${this.apiUrl}/amenities`);
  }

  getRoomTypes() {
    return this.http.get<RoomType[]>(`${this.apiUrl}/RoomTypes`);
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
        const drafts = listings.filter(listing => listing.verificationStatusId === 1);
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
    return this.http.post<Listing>(`${this.apiUrl}/listings/empty`, {}).pipe(
      tap(listing => {
        this.hostDraftsSignal.update(drafts => [...drafts, listing]);
      }),
      catchError(error => {
        return of(null);
      })
    )
  }

  updateListingStatus(id: string, verificationStatusId: {verificationStatusId: number}) {
    return this.http.put<{verificationStatusId: number}>(`${this.apiUrl}/listings/${id}/update-verification`, verificationStatusId);
  }

  updateListing(id: string, listing: NewListing) {
    return this.http.put<NewListing>(`${this.apiUrl}/listings/${id}`, listing);
  }

  uploadListingPhoto(listingId: string, file: File) {
    const formData = new FormData();
    formData.append('photo', file);
    return this.http.post<{ photoUrl: string }>(`${this.apiUrl}/Listings/${listingId}/photos`, formData);
  }

}
