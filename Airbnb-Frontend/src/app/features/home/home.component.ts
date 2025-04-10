import { Component, signal } from '@angular/core';
import { ListingCardComponent } from '../listing-card/listing-card.component';
import { Listing } from './../../core/models/Listing';
import { Subscription } from 'rxjs';
import { ListingsService } from '../../core/services/listings.service';

@Component({
  selector: 'app-home',
  standalone:true,
  imports: [ListingCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
  })
export class HomeComponent {
  constructor(private listingsService: ListingsService) {}
  listingItems: Listing[] = [];
  loading = false;
  error: string | null = null;
  private subscription: Subscription | null = null;



  ngOnInit() {
    this.loading = true;
    this.subscription = this.listingsService.getListings().subscribe({
      next: (data) => {
        this.listingItems = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load listings';
        this.loading = false;
      }
    });
  }

  ngOnDestroy() {
    this.subscription?.unsubscribe();
  }

}
