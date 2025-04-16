import { Component, OnDestroy, OnInit } from '@angular/core';
import { ListingsService } from '../../../core/services/listings.service';
import { Subscription } from 'rxjs';
import { Listing } from '../../../core/models/Listing';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-host-drafts',
  imports: [CommonModule, RouterModule],
  templateUrl: './host-drafts.component.html',
  styleUrl: './host-drafts.component.css'
})
export class HostDraftsComponent implements OnInit, OnDestroy {
  draftListings: Listing[] = [];
  loading = false;
  error: string | null = null;
  private subscription = new Subscription();

  constructor(private listingsService: ListingsService) {}

  ngOnInit() {
    // this.loading = true;
    // this.subscription.add(
    //   this.listingsService.getDraftListings().subscribe({
    //     next: (listings) => {
    //       this.draftListings = listings;
    //       this.loading = false;
    //     },
    //     error: (error) => {
    //       console.error('Error loading draft listings:', error);
    //       this.error = 'Failed to load your draft listings';
    //       this.loading = false;
    //     }
    //   })
    // );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
