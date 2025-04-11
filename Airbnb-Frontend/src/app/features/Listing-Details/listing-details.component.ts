import { Listing } from './../../core/models/Listing';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ListingsService } from '../../core/services/listings.service';
import { Subscription } from 'rxjs';
import { GalleriaModule } from 'primeng/galleria';
import { DividerModule } from 'primeng/divider';


@Component({
  selector: 'app-listing-details',
  imports: [GalleriaModule , DividerModule],
  templateUrl: './listing-details.component.html',
  styleUrl: './listing-details.component.css'
})
export class ListingDetailsComponent implements OnInit , OnDestroy{

    constructor(private listingsService: ListingsService) {

    }
  @Input() listing:Listing = {} as Listing
  loading:boolean = false;
  error: string | null = null;
  private subscription: Subscription | null = null;
  showAllPhotos: boolean = false;
  showAllAmenities:boolean=false;
  responsiveOptions!: any[];


  ngOnInit(): void {
      this.loading=true;
      this.subscription = this.listingsService.getListingById(this.listing.id).subscribe({
        next: (data) => {
          this.listing=data;
          this.loading=false;
        },
        error: () =>{
          this.error = "failed to load the details of this listing";
          this.loading=false;
        } 
      })
        }

    ngOnDestroy(): void {
      this.subscription?.unsubscribe();
    }
  




//   ngOnInit() {
//     // In a real app, you would fetch this data from a service
//     this.listing = {
//       id: 1,
//       title: 'Mountain View Cabin with Hot Tub',
//       location: 'Big Bear, California, United States',
//       rating: 4.98,
//       reviews: 124,
//       host: {
//         name: 'Hosted by Emily',
//         isSuperhost: true,
//         photo: 'https://randomuser.me/api/portraits/women/44.jpg',
//         joinedDate: 'January 2018'
//       },
//       details: {
//         guests: 6,
//         bedrooms: 2,
//         beds: 3,
//         baths: 2
//       },
//       highlights: [
//         'Entire home to yourself',
//         'Self check-in',
//         'Free cancellation for 48 hours',
//         'Superhost with high ratings'
//       ],
//       description: 'Escape to this beautiful cabin nestled in the woods with stunning mountain views. The cabin features modern amenities while maintaining rustic charm. Enjoy the private hot tub on the deck, cozy up by the fireplace, or explore nearby hiking trails. Located just 10 minutes from downtown Big Bear and 15 minutes from Big Bear Lake.',
//       price: 189,
//       cleaningFee: 85,
//       serviceFee: 45,
//       amenities: [
//         { name: 'Hot tub', icon: 'pi pi-heart-fill' },
//         { name: 'Mountain view', icon: 'pi pi-camera' },
//         { name: 'Wifi', icon: 'pi pi-wifi' },
//         { name: 'Free parking', icon: 'pi pi-car' },
//         { name: 'Kitchen', icon: 'pi pi-home' },
//         { name: 'Washer', icon: 'pi pi-spin' },
//         { name: 'Dryer', icon: 'pi pi-spin' },
//         { name: 'Air conditioning', icon: 'pi pi-sun' },
//         { name: 'Heating', icon: 'pi pi-sun' },
//         { name: 'Smoke alarm', icon: 'pi pi-exclamation-triangle' },
//         { name: 'Carbon monoxide alarm', icon: 'pi pi-exclamation-circle' },
//         { name: 'First aid kit', icon: 'pi pi-plus' }
//       ],
//       images: [
//         { source: 'https://images.unsplash.com/photo-1542718610-a1d656d1884c', alt: 'Cabin exterior' },
//         { source: 'https://images.unsplash.com/photo-1593696140826-c58b021acf8b', alt: 'Living room' },
//         { source: 'https://images.unsplash.com/photo-1560448204-603b3fc33ddc', alt: 'Kitchen' },
//         { source: 'https://images.unsplash.com/photo-1551105378-78e609e1d468', alt: 'Bedroom' },
//         { source: 'https://images.unsplash.com/photo-1507652313519-d4e9174996dd', alt: 'Bathroom' },
//         { source: 'https://images.unsplash.com/photo-1599327286062-30cfa4e2d34f', alt: 'Hot tub' },
//         { source: 'https://images.unsplash.com/photo-1521401830884-6c03c1c87ebb', alt: 'View from deck' },
//         { source: 'https://images.unsplash.com/photo-1521401830884-6c03c1c87ebb', alt: 'Exterior night' }
//       ]
//     };

//     // Mock reviews
//     this.reviews = [
//       {
//         id: 1,
//         user: 'Sarah',
//         date: 'March 2025',
//         avatar: 'https://randomuser.me/api/portraits/women/22.jpg',
//         rating: 5,
//         comment: 'Absolutely amazing place! The cabin was spotless and the views are breathtaking. The hot tub was perfect after a day of hiking. We'
//       },
//       {
//         id: 2,
//         user: 'Michael',
//         date: 'February 2025',
//         avatar: 'https://randomuser.me/api/portraits/men/32.jpg',
//         rating: 5,
//         comment: 'Great location and beautiful cabin. Everything was exactly as described. Emily was very responsive and helpful throughout our stay.'
//       },
//       {
//         id: 3,
//         user: 'Jessica',
//         date: 'January 2025',
//         avatar: 'https://randomuser.me/api/portraits/women/56.jpg',
//         rating: 4,
//         comment: 'We had a wonderful weekend getaway. The cabin is cozy and has everything you need. The only small issue was the slow wifi, but we came to disconnect anyway!'
//       }
//     ];
//   }

//   calculateTotalPrice() {
//     if (this.checkInDate && this.checkOutDate) {
//       const millisecondsPerDay = 24 * 60 * 60 * 1000;
//       this.nights = Math.round(Math.abs((this.checkOutDate.getTime() - this.checkInDate.getTime()) / millisecondsPerDay));
      
//       if (this.nights > 0) {
//         const stayPrice = this.listing.price * this.nights;
//         this.totalPrice = stayPrice + this.listing.cleaningFee + this.listing.serviceFee;
//         return true;
//       }
//     }
//     this.totalPrice = 0;
//     this.nights = 0;
//     return false;
//   }

  toggleAmenities() {
    this.showAllAmenities = !this.showAllAmenities;
  }

  togglePhotos() {
    this.showAllPhotos = !this.showAllPhotos;
  }

  reserveNow() {
    // In a real app, this would send reservation data to a service
    alert('Your reservation has been confirmed!');
  }

//   // Date selection validation
//   onDateSelect() {
//     if (this.checkInDate && this.checkOutDate) {
//       if (this.checkOutDate < this.checkInDate) {
//         this.checkOutDate = null;
//         alert('Check-out date cannot be before check-in date');
//       } else {
//         this.calculateTotalPrice();
//       }
//     }
//   }
// }
}