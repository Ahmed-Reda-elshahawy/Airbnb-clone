import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { Table, TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ProgressBar } from 'primeng/progressbar';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { Representative } from '../../../core/models/user';
import { ListingsService } from '../../../core/services/listings.service';
import { Listing } from '../../../core/models/Listing';
import { Subscription } from 'rxjs';
import { AddListingComponent } from "../add-listing/add-listing.component";
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-host-listing',
  imports: [TableModule, CommonModule, InputTextModule, TagModule, FormsModule, RouterLink,
  SelectModule, MultiSelectModule, ButtonModule, IconFieldModule, InputIconModule],
  templateUrl: './host-listing.component.html',
  styleUrl: './host-listing.component.css'
})
export class HostListingComponent implements OnInit, OnDestroy {
  constructor(public listingsService: ListingsService, private authService: AuthService) { }
  // listings: Listing[] = [];
  representatives!: Representative[];
  statuses!: any[];
  loading: boolean = false;
  activityValues: number[] = [0, 100];
  searchValue: string | undefined;
  subscription: Subscription = new Subscription();

  ngOnInit() {
    this.loading = true;
    console.log(this.authService.currentUserSignal()?.id);
    this.subscription.add(
      this.listingsService.getListingsByHostId(this.authService.currentUserSignal()?.id ?? "").subscribe({
        next: (listings) => {
          // this.listings = listings;
          this.loading = false;
        },
        error: (error) => {
          console.log('Error fetching listings:', error);
          this.loading = false;
        }
      })
    );
  }

  clear(table: Table) {
    table.clear();
    this.searchValue = ''
  }

  getStatus(statusNumber: number) {
    switch (statusNumber) {
      case 1:
        return 'Pending';

      case 2:
        return 'Verified';

      case 3:
        return 'Rejected';

      default:
        return "Pending";
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
