import { Component, inject, Input, signal } from '@angular/core';
import { ListingCardComponent } from '../listing-card/listing-card.component';
import { Listing } from './../../core/models/Listing';
import { Subscription } from 'rxjs';
import { ListingsService } from '../../core/services/listings.service';
import { PropertyTypeService } from '../../core/services/property-type.service';
import { CarouselBasicDemo } from "../property-type/property-type.component";

@Component({
  selector: 'app-home',
  standalone:true,
  imports: [ListingCardComponent, CarouselBasicDemo],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
  })
export class HomeComponent {
  constructor(private listingsService: ListingsService) {}
  private readonly _propertyTypeService = inject(PropertyTypeService);
   listingItems: Listing[] = [];
   filteredListings: Listing[] = [];
  loading = false;
  error: string | null = null;
  private subscription: Subscription | null = null;



  ngOnInit() {
    this.loading = true;
    
this._propertyTypeService.getAllPropertyTypes().subscribe({
  next:(p)=>{
    console.log(p);
  },
  error:(err)=>{
    console.error(err); 
  }
})


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


  // filterListings(propertyTypeId: number) {
  //   this.filteredListings = this.listingItems.filter(
  //     (listing) => listing.propertyTypeId === propertyTypeId
  //   );
  // }

}
