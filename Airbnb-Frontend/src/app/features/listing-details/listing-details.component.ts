import { Listing } from './../../core/models/Listing';
import { AfterViewInit, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ListingsService } from '../../core/services/listings.service';
import { Subscription } from 'rxjs';
import { GalleriaModule } from 'primeng/galleria';
import { DividerModule } from 'primeng/divider';
import { ButtonModule } from 'primeng/button';
import { ProgressBarModule } from 'primeng/progressbar';
import * as L from 'leaflet';

@Component({
  selector: 'app-listing-details',
  imports: [GalleriaModule , DividerModule ,ButtonModule,ProgressBarModule ],
  templateUrl: './listing-details.component.html',
  styleUrl: './listing-details.component.css',
})
export class ListingDetailsComponent implements OnInit , OnDestroy ,AfterViewInit{

    constructor(private listingsService: ListingsService ) {


    }

  @Input() listing:Listing = {} as Listing
  loading:boolean = false;
  error: string | null = null;
  private subscription: Subscription | null = null;
  showAllPhotos: boolean = false;
  showAllAmenities: boolean = false;
  activeImageIndex: number = 0;
  responsiveOptions!: any[];
  visibleAmenities:any[]=[];
  defaultVisibleCount:number = 3;



  ngOnInit(): void {
    this.loading = true;
    this.subscription = this.listingsService
      .getListingById(this.listing.id)
      .subscribe({
        next: (data) => {
          this.listing=data;
          this.loading=false;
          // this.initMap(); 
        },
        error: () => {
          this.error = 'failed to load the details of this listing';
          this.loading = false;
        },
      });

    this.updateVisibleAmenities();
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  // ngOnInit() {
  //   // In a real app, you would fetch this data from a service
 
// @Input() listing:Listing = {
//   id: "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
//   hostId: "h12345678-9012-3456-7890-123456789012",
//   title: "Modern Downtown Apartment with Great View",
//   description: "Fully furnished contemporary apartment in prime location, close to all transportation and tourist attractions. Perfect for families or groups of friends. Fully furnished contemporary apartment in prime location, close to all transportation and tourist attractions. Perfect for families or groups of friends. Fully furnished contemporary apartment in prime location, close to all transportation and tourist attractions. Perfect for families or groups of friends.",
//   propertyTypeId: 1, // Apartment
//   roomTypeId: 2, // Entire place
//   capacity: 4,
//   bedrooms: 2,
//   bathrooms: 1,
//   pricePerNight: 120,
//   serviceFee: 20,
//   addressLine1: "123 Palm Street",
//   addressLine2: "3rd Floor",
//   city: "New York",
//   state: "NY",
//   country: "USA",
//   postalCode: "10001",
//   latitude: 40.7128,
//   longitude: -74.0060,
//   instantBooking: true,
//   createdAt: new Date("2023-01-15"),
//   updatedAt: new Date("2023-06-20"),
//   minNights: 2,
//   maxNights: 30,
//   cancellationPolicyId: 2, // Flexible policy
//   averageRating: 4.8,
//   reviewCount: 24,
//   isActive: true,
//   currencyId: 1, // USD
//   imageUrls: [
// "https://images.unsplash.com/photo-1507652313519-d4e9174996dd", 
// 'https://images.unsplash.com/photo-1593696140826-c58b021acf8b',
// 'https://images.unsplash.com/photo-1521401830884-6c03c1c87ebb',
// 'https://images.unsplash.com/photo-1560448204-603b3fc33ddc', 
// 'https://images.unsplash.com/photo-1551105378-78e609e1d468', 
// 'https://images.unsplash.com/photo-1507652313519-d4e9174996dd',
// 'https://images.unsplash.com/photo-1521401830884-6c03c1c87ebb',

//   ],
//   previewImageUrl: "https://example.com/images/listing1_preview.jpg",
//    amenities: [
//     {
//       id: "amenity1",
//       name: "WiFi",
//       categoryId: "cat1",
//       icon: "wifi",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity2",
//       name: "Air Conditioning",
//       categoryId: "cat2",
//       icon: "air-conditioning",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     },
//     {
//       id: "amenity3",
//       name: "Free Parking",
//       categoryId: "cat3",
//       icon: "parking",
//       createdAt: new Date("2023-01-10")
//     }
//   ],
//    reviews: [
//     {
//       comment: "Great place! Very clean and accurate to the description.",
//       createdAt: new Date("2023-05-15"),
//       cleanlinessRating: 5,
//       accuracyRating: 5,
//       communicationRating: 4,
//       locationRating: 4,
//       checkInRating: 5,
//       valueRating: 4,
//       rating: 4.5
//     },
//     {
//       comment: "Nice location but could be cleaner.",
//       createdAt: new Date("2023-06-20"),
//       cleanlinessRating: 3,
//       accuracyRating: 4,
//       communicationRating: 5,
//       locationRating: 5,
//       checkInRating: 4,
//       valueRating: 3,
//       rating: 4
//     },
//     {
//       comment: "Excellent communication from the host!",
//       createdAt: new Date("2023-07-10"),
//       cleanlinessRating: 4,
//       accuracyRating: 4,
//       communicationRating: 5,
//       locationRating: 3,
//       checkInRating: 5,
//       valueRating: 4,
//       rating: 4.2
//     }
//   ]
// } as Listing;





clean(){
  let x =0
  for (let index = 0; index < this.listing.reviews.length; index++) {
     x+= this.listing.reviews[index].cleanlinessRating
  }
  return Math.ceil(x/this.listing.reviews.length *10)/10;
}

Accuracy(){
  let x =0
  for (let index = 0; index < this.listing.reviews.length; index++) {
     x+= this.listing.reviews[index].accuracyRating
  }
  return Math.ceil(x/this.listing.reviews.length *10)/10;
}

checkin(){
  let x =0
  for (let index = 0; index < this.listing.reviews.length; index++) {
     x+= this.listing.reviews[index].checkInRating
  }
  return Math.ceil(x/this.listing.reviews.length *10)/10;
}
communication(){
  let x =0
  for (let index = 0; index < this.listing.reviews.length; index++) {
     x+= this.listing.reviews[index].communicationRating
  }
  return Math.ceil(x/this.listing.reviews.length *10)/10;
}
location(){
  let x =0
  for (let index = 0; index < this.listing.reviews.length; index++) {
     x+= this.listing.reviews[index].locationRating
  }
  return Math.ceil(x/this.listing.reviews.length *10)/10;
}
value(){
  let x =0
  for (let index = 0; index < this.listing.reviews.length; index++) {
     x+= this.listing.reviews[index].valueRating
  }
  return Math.ceil(x/this.listing.reviews.length *10)/10;
}

ratingToPercentage(rating: number): number {
  return   ((rating / 5) * 100);
}



ratingCategories = [
  { name: 'Cleanliness', rating: this.clean()},
  { name: 'Accuracy', rating: this.Accuracy()},
  { name: 'Check-in', rating: this.checkin() },
  { name: 'Communication', rating:this.communication() },
  { name: 'Location', rating: this.location()},
  { name: 'Value', rating: this.value()},
  
];



  //   // Mock reviews
  //   this.reviews = [
  //     {
  //       id: 1,
  //       user: 'Sarah',
  //       date: 'March 2025',
  //       avatar: 'https://randomuser.me/api/portraits/women/22.jpg',
  //       rating: 5,
  //       comment: 'Absolutely amazing place! The cabin was spotless and the views are breathtaking. The hot tub was perfect after a day of hiking. We'
  //     },
  //     {
  //       id: 2,
  //       user: 'Michael',
  //       date: 'February 2025',
  //       avatar: 'https://randomuser.me/api/portraits/men/32.jpg',
  //       rating: 5,
  //       comment: 'Great location and beautiful cabin. Everything was exactly as described. Emily was very responsive and helpful throughout our stay.'
  //     },
  //     {
  //       id: 3,
  //       user: 'Jessica',
  //       date: 'January 2025',
  //       avatar: 'https://randomuser.me/api/portraits/women/56.jpg',
  //       rating: 4,
  //       comment: 'We had a wonderful weekend getaway. The cabin is cozy and has everything you need. The only small issue was the slow wifi, but we came to disconnect anyway!'
  //     }
  //   ];
  // }

  // calculateTotalPrice() {
  //   if (this.checkInDate && this.checkOutDate) {
  //     const millisecondsPerDay = 24 * 60 * 60 * 1000;
  //     this.nights = Math.round(Math.abs((this.checkOutDate.getTime() - this.checkInDate.getTime()) / millisecondsPerDay));

  //     if (this.nights > 0) {
  //       const stayPrice = this.listing.price * this.nights;
  //       this.totalPrice = stayPrice + this.listing.cleaningFee + this.listing.serviceFee;
  //       return true;
  //     }
  //   }
  //   this.totalPrice = 0;
  //   this.nights = 0;
  //   return false;
  // }

  updateVisibleAmenities() {
    this.visibleAmenities = this.showAllAmenities
      ? this.listing.amenities
      : this.listing.amenities.slice(0, 3);
  }

  toggleAmenities() {
    this.showAllAmenities = !this.showAllAmenities;
  }

  togglePhotos(index: number = 0) {
    this.activeImageIndex = index;
    this.showAllPhotos = !this.showAllPhotos;
  }

  reserveNow() {
    // In a real app, this would send reservation data to a service
    alert('Your reservation has been confirmed!');
  }

  // Date selection validation
  // onDateSelect() {
  //   if (this.checkInDate && this.checkOutDate) {
  //     if (this.checkOutDate < this.checkInDate) {
  //       this.checkOutDate = null;
  //       alert('Check-out date cannot be before check-in date');
  //     } else {
  //       this.calculateTotalPrice();
  //     }
  //   }
  // }
  // ratingCategories = [
  //   { name: 'Cleanliness', rating: 4.9, value: 98 },
  //   { name: 'Accuracy', rating: 4.8, value: 96 },
  //   { name: 'Check-in', rating: 5.0, value: 100 },
  //   { name: 'Communication', rating: 4.9, value: 98 },
  //   { name: 'Location', rating: 4.7, value: 94 },
  //   { name: 'Value', rating: 4.6, value: 92 },
    
  // ];

  ngAfterViewInit(): void {
    setTimeout(() => this.initMap(), 100);
  }
  map: any;
  initMap(): void {
    this.map = L.map('map').setView([this.listing.latitude, this.listing.longitude], 13);
    // setTimeout(() => {
    //   this.map.invalidateSize();
    // }, 0);
    
  
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);
  
    const customIcon = L.icon({
      iconUrl: 'assets/images/custom-marker.png',
      iconSize: [40, 40],
      iconAnchor: [20, 40],
      popupAnchor: [0, -35]
    });
  
    const marker = L.marker([30.0444, 31.2357], {
      icon: customIcon
    }).addTo(this.map);
  
    const popupContent = `
      <div style="text-align:center;">
        <img src="${this.listing.imageUrls[0]}" alt="Preview" style="width:100px; height:70px; object-fit:cover; border-radius:5px; margin-bottom:5px;" />
        <div style="font-weight:bold;">${this.listing.title}</div>
      </div>
    `;
  
    marker.bindPopup(popupContent).openPopup();

    setTimeout(() => {
      this.map.invalidateSize();
    }, 200);
  
  }
  


  
}





//--------------------------------------
//------------------------------------
//------------------------------------

// import { Listing } from './../../core/models/Listing';
// import { Component, Input, OnDestroy, OnInit } from '@angular/core';
// import { ListingsService } from '../../core/services/listings.service';
// import { Subscription } from 'rxjs';
// import { GalleriaModule } from 'primeng/galleria';
// import { DividerModule } from 'primeng/divider';
// import { ButtonModule } from 'primeng/button';
// import { DomSanitizer } from '@angular/platform-browser';

// @Component({
//   selector: 'app-listing-details',
//   imports: [GalleriaModule , DividerModule ,ButtonModule ],
//   templateUrl: './listing-details.component.html',
//   styleUrl: './listing-details.component.css'
// })
// export class ListingDetailsComponent implements OnInit , OnDestroy{

//     constructor(private listingsService: ListingsService , private sanitizer: DomSanitizer) {

//     }
//   // @Input() listing:Listing = {} as Listing
//   loading:boolean = false;
//   error: string | null = null;
//   private subscription: Subscription | null = null;
//   showAllPhotos: boolean = false;
//   showAllAmenities:boolean=false;
//   responsiveOptions!: any[];

//   cleanUrl(url: string) {
//     return this.sanitizer.bypassSecurityTrustUrl(
//       url.replace(/^"|"$/g,'') // يمسح أي إقتباسات
//     );
//   }

//   ngOnInit(): void {
//       this.loading=true;
//       this.subscription = this.listingsService.getListingById(this.listing.id).subscribe({
//         next: (data) => {
//           this.listing=data;
//           this.loading=false;
//         },
//         error: () =>{
//           this.error = "failed to load the details of this listing";
//           this.loading=false;
//         }
//       })
//         }

//     ngOnDestroy(): void {
//       this.subscription?.unsubscribe();
//     }

// @Input() listing:Listing = {
//   id: "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
//   hostId: "h12345678-9012-3456-7890-123456789012",
//   title: "Modern Downtown Apartment with Great View",
//   description: "Fully furnished contemporary apartment in prime location, close to all transportation and tourist attractions. Perfect for families or groups of friends.",
//   propertyTypeId: 1, // Apartment
//   roomTypeId: 2, // Entire place
//   capacity: 4,
//   bedrooms: 2,
//   bathrooms: 1,
//   pricePerNight: 120,
//   serviceFee: 20,
//   addressLine1: "123 Palm Street",
//   addressLine2: "3rd Floor",
//   city: "New York",
//   state: "NY",
//   country: "USA",
//   postalCode: "10001",
//   latitude: 40.7128,
//   longitude: -74.0060,
//   instantBooking: true,
//   createdAt: new Date("2023-01-15"),
//   updatedAt: new Date("2023-06-20"),
//   minNights: 2,
//   maxNights: 30,
//   cancellationPolicyId: 2, // Flexible policy
//   averageRating: 4.8,
//   reviewCount: 24,
//   isActive: true,
//   currencyId: 1, // USD
//   imageUrls: [
// "https://images.unsplash.com/photo-1507652313519-d4e9174996dd",
// 'https://images.unsplash.com/photo-1593696140826-c58b021acf8b',
// 'https://images.unsplash.com/photo-1560448204-603b3fc33ddc',
// 'https://images.unsplash.com/photo-1551105378-78e609e1d468',
// 'https://images.unsplash.com/photo-1507652313519-d4e9174996dd',
// 'https://images.unsplash.com/photo-1599327286062-30cfa4e2d34f',
// 'https://images.unsplash.com/photo-1521401830884-6c03c1c87ebb',
// 'https://images.unsplash.com/photo-1521401830884-6c03c1c87ebb',
//   ],
//   previewImageUrl: "https://example.com/images/listing1_preview.jpg",
//   //  amenities: [
//   //   {
//   //     id: "amenity1",
//   //     name: "WiFi",
//   //     categoryId: "cat1",
//   //     icon: "wifi",
//   //     createdAt: new Date("2023-01-10")
//   //   },
//   //   {
//   //     id: "amenity2",
//   //     name: "Air Conditioning",
//   //     categoryId: "cat2",
//   //     icon: "air-conditioning",
//   //     createdAt: new Date("2023-01-10")
//   //   },
//   //   {
//   //     id: "amenity3",
//   //     name: "Free Parking",
//   //     categoryId: "cat3",
//   //     icon: "parking",
//   //     createdAt: new Date("2023-01-10")
//   //   }
//   // ]
// } as Listing ;

//   toggleAmenities() {
//     this.showAllAmenities = !this.showAllAmenities;
//   }

//   togglePhotos() {
//     this.showAllPhotos = !this.showAllPhotos;
//   }

//   reserveNow() {
//     // In a real app, this would send reservation data to a service
//     alert('Your reservation has been confirmed!');
//   }

// }
