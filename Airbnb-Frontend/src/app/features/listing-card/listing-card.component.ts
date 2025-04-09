
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';


@Component({
  selector: 'app-listing-card',
  standalone: true,
  imports:[CommonModule],
  templateUrl: './listing-card.component.html',
  styleUrls: ['./listing-card.component.css']
})
export class ListingCardComponent {
  hover: boolean = false;
  images: string[] = [
    'https://dq5r178u4t83b.cloudfront.net/wp-content/uploads/sites/125/2020/06/15182916/Sofitel-Dubai-Wafi-Luxury-Room-Bedroom-Skyline-View-Image1_WEB.jpg',
    'https://www.usatoday.com/gcdn/authoring/authoring-images/2024/05/26/USAT/73865433007-tempoby-hilton-nashville-standard-king.jpg',
    'https://www.usatoday.com/gcdn/-mm-/05b227ad5b8ad4e9dcb53af4f31d7fbdb7fa901b/c=0-64-2119-1259/local/-/media/USATODAY/USATODAY/2014/08/13/1407953244000-177513283.jpg'
  ];


    // ... الكود الحالي ...
    isFavorite: boolean = false;
  
    toggleFavorite(event: Event) {
      event.preventDefault(); // إضافة هذه السطر لمنع السلوك الافتراضي
      event.stopPropagation(); // لمنع انتشار الحدث
      this.isFavorite = !this.isFavorite;
      console.log('Favorite status:', this.isFavorite); // للتأكد من أن الدالة تعمل
    }
    
  

  currentImageIndex = 0;

  nextImage() {
    if (this.currentImageIndex < this.images.length - 1) {
      this.currentImageIndex++;
    }
  }

  prevImage() {
    if (this.currentImageIndex > 0) {
      this.currentImageIndex--;
    }
  }

  // Format the date as "jun 14-17"
  getFormattedDate(): string {
    const startDate = new Date(2023, 5, 14); // June 14
    const endDate = new Date(2023, 5, 17); // June 17

    const options: Intl.DateTimeFormatOptions = { month: 'short', day: 'numeric' };

    const startDateStr = startDate.toLocaleDateString('en-US', options);
    const endDateStr = endDate.toLocaleDateString('en-US', options);

    return `${startDateStr}-${endDateStr}`;
  }

}






