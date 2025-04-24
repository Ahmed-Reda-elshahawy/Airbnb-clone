import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AdminStatisticsService {
  // URL for your backend API that returns chart data
  private apiUrl = 'https://localhost:7200/api/statistics';

  constructor(private http: HttpClient) {}
  getStatistics(endpoint: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${endpoint}`);
  }
  // Method to fetch chart data
  getNewBookingsVsCancellations(): Observable<any> {
    return this.getStatistics('bookings-per-month');  // Calling the generic method for bookings
  }
  getRevenue(): Observable<any> {
    return this.getStatistics('monthly-revenue');  // Calling the generic method for revenue
  }
  getUserDistribution(): Observable<any> {
    return this.getStatistics('role-distribution');
  }
  getTopHosts(): Observable<any> {
    return this.getStatistics('top-hosts');
  }
  getTopGuests(): Observable<any> {
    return this.http.get<any>('https://localhost:7200/api/statistics/top-guests');
  }
  getTopListings(): Observable<any> {
    return this.http.get<any>('https://localhost:7200/api/statistics/top-listings');
  }
  getTopCities(): Observable<any> {
    return this.http.get<any>('https://localhost:7200/api/statistics/top-cities');
  }
}
