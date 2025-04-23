

import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Listing } from './../../core/models/Listing';
import { ImagesService } from '../../core/services/images.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-listing-card',
  standalone: true,
  imports: [CommonModule],
  providers: [ImagesService],
  templateUrl: './listing-card.component.html',
  styleUrls: ['./listing-card.component.css']
})
export class ListingCardComponent {

  @Input() listingItem: Listing = {} as Listing;
  hover: boolean = false;
  isFavorite: boolean = false;
  currentImageIndex = 0;

  constructor(public imgsService: ImagesService, private router: Router) {}

  @Output() toggleWishList = new EventEmitter<string>();

  toggleFavorite(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    // this.isFavorite = !this.isFavorite;
    // this.toggleWishList.emit(this.listingItem.id);
    console.log('Favorite status:', this.isFavorite); // للتأكد من أن الدالة تعمل
  }







  nextImage() {
    if (this.currentImageIndex < this.listingItem.imageUrls.length - 1) {
      this.currentImageIndex++;
    }
  }

  prevImage() {
    if (this.currentImageIndex > 0) {
      this.currentImageIndex--;
    }
  }

  getFormattedDate(): string {
    // console.log(this.listingItem.createdAt)
    const apiDate=this.listingItem.createdAt;
    const dateObj = new Date(apiDate);

    const year=dateObj.getFullYear();
    const month=dateObj.getMonth();
    const day=dateObj.getDate();

    const startDate = new Date(year, month, day); // June 14
    const endDate = new Date(year, month, day); // June 17

    const options: Intl.DateTimeFormatOptions = { month: 'short', day: 'numeric' };
    const startDateStr = startDate.toLocaleDateString('en-US', options);
    const endDateStr = endDate.toLocaleDateString('en-US', options);
    return `${startDateStr}-${endDateStr}`;
  }
}






