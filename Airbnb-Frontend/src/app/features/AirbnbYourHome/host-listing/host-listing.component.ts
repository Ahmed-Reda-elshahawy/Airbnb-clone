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


    this.representatives = [
      { name: 'Amy Elsner', image: 'amyelsner.png' },
      { name: 'Anna Fali', image: 'annafali.png' },
      { name: 'Asiya Javayant', image: 'asiyajavayant.png' },
      { name: 'Bernardo Dominic', image: 'bernardodominic.png' },
      { name: 'Elwin Sharvill', image: 'elwinsharvill.png' },
      { name: 'Ioni Bowcher', image: 'ionibowcher.png' },
      { name: 'Ivan Magalhaes', image: 'ivanmagalhaes.png' },
      { name: 'Onyama Limba', image: 'onyamalimba.png' },
      { name: 'Stephen Shaw', image: 'stephenshaw.png' },
      { name: 'Xuxue Feng', image: 'xuxuefeng.png' }
    ];

    this.statuses = [
      { label: 'Unqualified', value: 'unqualified' },
      { label: 'Qualified', value: 'qualified' },
      { label: 'New', value: 'new' },
      { label: 'Negotiation', value: 'negotiation' },
      { label: 'Renewal', value: 'renewal' },
      { label: 'Proposal', value: 'proposal' }
    ];
  }

  clear(table: Table) {
    table.clear();
    this.searchValue = ''
  }

  getSeverity(status: string) {
    switch (status.toLowerCase()) {
      case 'unqualified':
        return 'danger';

      case 'qualified':
        return 'success';

      case 'new':
        return 'info';

      case 'negotiation':
        return 'warn';

      case 'renewal':
        return null;
      default:
        return null;
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
