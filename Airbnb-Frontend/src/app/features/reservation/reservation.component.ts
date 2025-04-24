import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ListingsService } from '../../core/services/listings.service';
import { Listing } from '../../core/models/Listing';
import { Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reservation',
  standalone: true,
  imports: [CommonModule, HttpClientModule, ReactiveFormsModule],
  templateUrl: './reservation.component.html',
  styleUrls: ['./reservation.component.css'],
})
export class ReservationComponent implements OnInit {
  reservationData: any;
  priceData = {
    nightlyRate: 13325.23,
    nights: 3,
    subtotal: 39975.70,
    serviceFee: 300,
    taxes: 150,
    totalAmount: 40425.70

  };

  isLoading = false;
  listings!: Listing;
  showInfoModal = false;
  activePaymentMethod: string = 'credit';
  selectedOption: string = 'now';
  paymentProcessing = false;
  paymentSuccess = false;
  testMode = false; // إضافة هذا المتغير للتحكم في وضع الاختبار

  order: FormGroup = new FormGroup({
    paywith: new FormControl('credit'),
    cardnumber: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{16}$/)
    ]),
    expiry: new FormControl('', [
      Validators.required,
      Validators.pattern(/^(0[1-9]|1[0-2])\/?([0-9]{2})$/)
    ]),
    cvv: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{3,4}$/)
    ]),
     nameoncard: new FormControl('', Validators.required),
    streetAddress: new FormControl('', Validators.required),
    aptSuite: new FormControl(''),
    city: new FormControl('', Validators.required),
    state: new FormControl('', Validators.required),
    zipcode: new FormControl('', Validators.required),
    country: new FormControl('Egypt', Validators.required)
  });

  constructor(
    private listingsService: ListingsService,
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadListings();
    this.setupFormListeners();
    this.selectOption(this.selectedOption);
  this.getReservationData();
    this.setupFormListeners();
    this.selectOption(this.selectedOption);
  }

  private formatDate(date: string): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
 getReservationData() {
    const navigation = this.router.getCurrentNavigation();
    this.reservationData = navigation?.extras.state?.['data'];

    if (this.reservationData) {
      console.log('Reservation data:', this.reservationData);
      this.listings = this.reservationData.listing;
      // يمكنك هنا تحديث priceData بناء على التواريخ وعدد الضيوف
    } else {
      console.error('No reservation data found');
      // يمكنك هنا إعادة التوجيه إلى الصفحة السابقة أو إظهار رسالة خطأ
    }
  }
 calculatePrice() {
    if (this.reservationData?.checkIn && this.reservationData?.checkOut) {
      const diffTime = Math.abs(
        new Date(this.reservationData.checkOut).getTime() -
        new Date(this.reservationData.checkIn).getTime()
      );
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

      this.priceData.nights = diffDays;
      this.priceData.nightlyRate = this.listings.pricePerNight;
      this.priceData.subtotal = this.listings.pricePerNight * diffDays;
      this.priceData.totalAmount = this.priceData.subtotal +
                                 this.priceData.serviceFee +
                                 this.priceData.taxes;
    }
  }
  getStars(rating: number): string[] {
    const fullStars = Math.floor(rating);
    const halfStar = rating % 1 >= 0.5;
    const emptyStars = 5 - fullStars - (halfStar ? 1 : 0);

    const stars = [];
    for (let i = 0; i < fullStars; i++) {
      stars.push('full');
    }
    if (halfStar) {
      stars.push('half');
    }
    for (let i = 0; i < emptyStars; i++) {
      stars.push('empty');
    }
    return stars;
  }
  loadListings() {
  const listingId = this.route.snapshot.paramMap.get('id');

  if (!listingId) {
    console.error('No listing ID provided');
    return;
  }

  this.listingsService.getListingById(listingId).subscribe({
    next: (data) => {
      this.listings = data;
      console.log('Listings loaded:', this.listings);
    },
    error: (err) => {
      console.error('Error loading listings:', err);
    }
  });
}

  setupFormListeners() {
    this.order.get('paywith')?.valueChanges.subscribe(value => {
      this.activePaymentMethod = value;
    });
  }


async orderSubmit() {
  if (this.order.invalid) {
    this.markAllAsTouched();
    return;
  }

  this.paymentProcessing = true;

  try {
    // 1. إنشاء الحجز أولاً
    const reservationResponse = await this.createReservation();

    // 2. معالجة الدفع
    const paymentResponse = await this.processPayment(reservationResponse.reservationId);

    // 3. إذا نجح الدفع، تأكيد الحجز
    await this.confirmBooking(reservationResponse.reservationId);

    this.paymentSuccess = true;
    alert('Payment successful!');
  } catch (error) {
    console.error('Payment failed:', error);
    const errorMessage = (error as { message: string }).message || 'Unknown error';
    alert('Payment failed: ' + errorMessage);
  } finally {
    this.paymentProcessing = false;
  }
}

private async createReservation() {
  const reservationData = {
        listingId: this.listings.id,
    checkIn: this.formatDate(this.reservationData.checkIn),
    checkOut: this.formatDate(this.reservationData.checkOut),
    guests: this.reservationData.guests,
    paymentMethod: this.order.value.paywith,
    totalAmount: this.priceData.totalAmount
  };

  return this.http.post<any>('https://localhost:7200/api/reservations', reservationData).toPromise();
  }
  private async processPayment(reservationId: string) {
  const paymentData = {
    reservationId: reservationId,
    cardNumber: this.order.value.cardnumber,
    expiry: this.order.value.expiry,
    cvv: this.order.value.cvv,
    amount: this.priceData.totalAmount,
    currency: 'EGP'
  };
  return this.http.post<any>('https://localhost:7200/api/payment', paymentData).toPromise();
  }
  private async confirmBooking(reservationId: string) {
  return this.http.post<any>(`https://localhost:7200/api/reservations/${reservationId}/confirm`, {}).toPromise();
}



  private markAllAsTouched() {
    Object.values(this.order.controls).forEach(control => {
      control.markAsTouched();
    });
  }

  selectOption(option: string): void {
    this.selectedOption = option;
    if (option === 'later') {
      this.priceData.totalAmount = this.priceData.subtotal * 0.5;
    } else {
      this.priceData.totalAmount = this.priceData.subtotal;
    }
  }

  onPaymentMethodChange(event: any): void {
    this.activePaymentMethod = event.target.value;
    this.order.get('paywith')?.setValue(event.target.value);
  }

}
