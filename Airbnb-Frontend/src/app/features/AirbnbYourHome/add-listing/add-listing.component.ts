import { CommonModule, Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ListingsService } from '../../../core/services/listings.service';
import { Subscription, forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';
import { PropertyType } from '../../../core/models/PropertyType';
import { RoomType } from '../../../core/models/RoomType';
import { Amenity } from '../../../core/models/Amenity';
import { DragDropModule, CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

interface UploadedImage {
  file: File;
  preview: string;
}

interface FormStep {
  controlName: string;
  isValid: () => boolean;
}

@Component({
  selector: 'app-add-listing',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DragDropModule],
  templateUrl: './add-listing.component.html',
  styleUrl: './add-listing.component.css'
})
export class AddListingComponent implements OnInit, OnDestroy {
  private subscriptions = new Subscription();
  currentStep = 1;
  totalSteps = 10;

  formSteps: FormStep[] = [
    { controlName: 'propertyType', isValid: () => this.listingForm.get('propertyType')?.valid ?? false },
    { controlName: 'roomType', isValid: () => this.listingForm.get('roomType')?.valid ?? false },
    { controlName: 'amenityType', isValid: () => this.listingForm.get('amenityType')?.valid ?? false },
    {
      controlName: 'address',
      isValid: () => ['country', 'streetAddress', 'city', 'state', 'postalCode']
        .every(ctrl => this.listingForm.get(ctrl)?.valid ?? false)
    },
    {
      controlName: 'titleDesc',
      isValid: () => ['title', 'description']
        .every(ctrl => this.listingForm.get(ctrl)?.valid ?? false)
    },
    {
      controlName: 'pricing',
      isValid: () => ['pricePerNight']
        .every(ctrl => this.listingForm.get(ctrl)?.valid ?? false)
    },
    {
      controlName: 'duration',
      isValid: () => ['minNights', 'maxNights']
        .every(ctrl => this.listingForm.get(ctrl)?.valid ?? false)
    },
    { controlName: 'cancellationPolicyId', isValid: () => this.listingForm.get('cancellationPolicyId')?.valid ?? false },
    {
      controlName: 'capacity',
      isValid: () => ['guests', 'bedrooms', 'bathrooms']
        .every(ctrl => this.listingForm.get(ctrl)?.valid ?? false)
    },
    { controlName: 'images', isValid: () => this.uploadedImages.length >= 4 }
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private listingsService: ListingsService
  ) { }

  listingId!: string;
  formErrors: any = {};
  PropertyTypes: PropertyType[] = [];
  RoomTypes: RoomType[] = [];
  Amenities: Amenity[] = [];
  CancellationPolicies = [
    { id: 1, name: 'Flexible' },
    { id: 2, name: 'Moderate' },
    { id: 3, name: 'Strict' },
    { id: 4, name: 'Super Strict 30 Days' },
    { id: 5, name: 'Super Strict 60 Days' }
  ];
  isLoading = false;
  listingForm!: FormGroup;
  uploadedImages: UploadedImage[] = [];

  get amenityTypeArray() {
    return this.listingForm.get('amenityType') as FormArray;
  }

  toggleAmenity(amenityId: string): void {
    const index = this.amenityTypeArray.value.indexOf(amenityId);
    if (index === -1) {
      this.amenityTypeArray.push(this.fb.control(amenityId));
    } else {
      this.amenityTypeArray.removeAt(index);
    }
  }

  isAmenitySelected(amenityId: string): boolean {
    return this.amenityTypeArray.value.includes(amenityId);
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.params.subscribe(params => {
        this.listingId = params['id'];
        if (this.listingId) {
          this.loadListingData();
        }
        this.loadAllPropertyTypes();
        this.loadAllRoomTypes();
        this.loadAllAmenities();
      })
    );

    this.listingForm = this.fb.group({
      propertyType: ['', Validators.required],
      roomType: ['', Validators.required],
      amenityType: this.fb.array([], [Validators.required, Validators.minLength(1)]),
      country: ['', Validators.required],
      streetAddress: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postalCode: ['', Validators.required],
      title: ['', [Validators.required, Validators.maxLength(32)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      guests: [1, [Validators.required, Validators.min(1), Validators.max(16)]],
      bedrooms: [1, [Validators.required, Validators.min(1), Validators.max(8)]],
      bathrooms: [1, [Validators.required, Validators.min(0.5), Validators.max(8)]],
      cancellationPolicyId: ['', Validators.required],
      pricePerNight: [50, [Validators.required, Validators.min(10)]],
      serviceFee: [0, [Validators.min(0)]],
      securityDeposit: [0, [Validators.min(0)]],
      minNights: [1, [Validators.required, Validators.min(1)]],
      maxNights: [30, [Validators.required, Validators.min(1)]],
      images: [[], [Validators.required, Validators.minLength(4)]]
    });

    this.route.queryParamMap.subscribe(params => {
      const draftId = params.get('draftId');
      if (draftId) {
        this.listingId = draftId;
        this.loadListingData();
      }
    });
  }

  loadListingData(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.listingsService.getListingById(this.listingId).subscribe({
        next: (data) => {
          if (data) {
            // Clear existing amenities array
            while (this.amenityTypeArray.length) {
              this.amenityTypeArray.removeAt(0);
            }

            // Add each amenity ID to the FormArray
            if (data.amenities && data.amenities.length > 0) {
              data.amenities.forEach(amenity => {
                this.amenityTypeArray.push(this.fb.control(amenity.id));
              });
            }

            this.listingForm.patchValue({
              propertyType: data.propertyTypeId,
              roomType: data.roomTypeId,
              country: data.country,
              streetAddress: data.addressLine1,
              city: data.city,
              state: data.state,
              postalCode: data.postalCode,
              title: data.title,
              description: data.description,
              guests: data.capacity,
              bedrooms: data.bedrooms,
              bathrooms: data.bathrooms,
              pricePerNight: data.pricePerNight,
              serviceFee: data.serviceFee,
              minNights: data.minNights,
              maxNights: data.maxNights,
              cancellationPolicyId: data.cancellationPolicy.id
            });
            this.isLoading = false;
          }
        },
        error: (error) => {
          console.error('Error loading listing data', error.error.message);
          this.formErrors['general'] = 'Failed to load listing data';
          this.isLoading = false;
        }
      })
    );
  }

  loadAllPropertyTypes(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.listingsService.getPropertyTypes().subscribe({
        next: (data) => {
          this.PropertyTypes = data;
        },
        error: (error) => {
          console.log('Error loading property types', error.error.message);
          this.formErrors['general'] = 'Failed to load property types';
          this.isLoading = false;
        }
      })
    )
  }

  loadAllRoomTypes(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.listingsService.getRoomTypes().subscribe({
        next: (data) => {
          this.RoomTypes = data;
        },
        error: (error) => {
          console.log('Error loading rooms', error.error.message);
          this.formErrors['general'] = 'Failed to load rooms';
          this.isLoading = false;
        }
      })
    )
  }

  loadAllAmenities(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.listingsService.getAmenities().subscribe({
        next: (data) => {
          this.Amenities = data;
        },
        error: (error) => {
          console.log('Error loading amenities', error.error.message);
          this.formErrors['general'] = 'Failed to load amenities';
          this.isLoading = false;
        }
      })
    )
  }

  incrementGuests(): void {
    const currentValue = this.listingForm.get('guests')?.value || 1;
    this.listingForm.patchValue({ guests: currentValue + 1 });
  }

  decrementGuests(): void {
    const currentValue = this.listingForm.get('guests')?.value || 1;
    if (currentValue > 1) {
      this.listingForm.patchValue({ guests: currentValue - 1 });
    }
  }

  incrementBedrooms(): void {
    const currentValue = this.listingForm.get('bedrooms')?.value || 1;
    this.listingForm.patchValue({ bedrooms: currentValue + 1 });
  }

  decrementBedrooms(): void {
    const currentValue = this.listingForm.get('bedrooms')?.value || 1;
    if (currentValue > 1) {
      this.listingForm.patchValue({ bedrooms: currentValue - 1 });
    }
  }

  incrementBathrooms(): void {
    const currentValue = this.listingForm.get('bathrooms')?.value || 1;
    this.listingForm.patchValue({ bathrooms: currentValue + 1 });
  }

  decrementBathrooms(): void {
    const currentValue = this.listingForm.get('bathrooms')?.value || 1;
    if (currentValue > 1) {
      this.listingForm.patchValue({ bathrooms: currentValue - 1 });
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    const files = event.dataTransfer?.files;
    if (files) {
      this.handleFiles(Array.from(files));
    }
  }

  onFileSelected(event: Event): void {
    const element = event.target as HTMLInputElement;
    const files = element.files;
    if (files) {
      this.handleFiles(Array.from(files));
    }
  }

  private handleFiles(files: File[]): void {
    const validFiles = files.filter(file => {
      const isValid = file.type.startsWith('image/') && file.size <= 10 * 1024 * 1024; // 10MB limit
      if (!isValid) {
        console.error(`Invalid file: ${file.name}. Must be an image under 10MB.`);
      }
      return isValid;
    });

    validFiles.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        const preview = e.target?.result as string;
        this.uploadedImages.push({ file, preview });
        this.listingForm.patchValue({
          images: this.uploadedImages
        });
      };
      reader.readAsDataURL(file);
    });
  }

  removeImage(index: number): void {
    this.uploadedImages.splice(index, 1);
    this.listingForm.patchValue({
      images: this.uploadedImages
    });
  }

  drop(event: CdkDragDrop<string[]>): void {
    moveItemInArray(this.uploadedImages, event.previousIndex, event.currentIndex);
    this.listingForm.patchValue({
      images: this.uploadedImages
    });
  }

  nextStep(): void {
    if (this.currentStep < this.totalSteps && this.isCurrentStepValid()) {
      this.currentStep++;
    }
  }

  previousStep(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  isCurrentStepValid(): boolean {
    return this.formSteps[this.currentStep - 1].isValid();
  }

  saveDraft(): void {
    if (this.listingForm.dirty) {
      const formData = this.listingForm.value;
      const listing = this.prepareListing(formData);

      this.subscriptions.add(
        this.listingsService.updateListing(this.listingId, listing).subscribe({
          next: () => {
            this.router.navigate(['../'], { relativeTo: this.route });
          },
          error: (error) => {
            console.error('Error saving draft', error);
            this.formErrors['submit'] = 'Failed to save draft';
          }
        })
      );
    }
  }

  private prepareListing(formData: any) {
    return {
      minNights: formData.minNights,
      maxNights: formData.maxNights,
      cancellationPolicyId: formData.cancellationPolicyId,
      title: formData.title,
      description: formData.description,
      propertyTypeId: formData.propertyType,
      roomTypeId: formData.roomType,
      capacity: formData.guests,
      bedrooms: formData.bedrooms,
      bathrooms: formData.bathrooms,
      pricePerNight: formData.pricePerNight,
      securityDeposit: formData.securityDeposit,
      serviceFee: formData.serviceFee,
      addressLine1: formData.streetAddress,
      addressLine2: "",
      city: formData.city,
      state: formData.state,
      country: formData.country,
      postalCode: formData.postalCode,
      instantBooking: true,
      latitude: 0,
      longitude: 0,
      currencyId: 1,
      amenityIds: formData.amenityType || []
    };
  }

  onSubmit(): void {
    if (this.listingForm.valid) {
      const formData = this.listingForm.value;
      this.isLoading = true;

      const listing = this.prepareListing(formData);

      this.subscriptions.add(
        this.listingsService.updateListing(this.listingId, listing).subscribe({
          next: () => {
            if (this.uploadedImages.length > 0) {
              this.uploadImages(this.listingId);
              console.log('Images uploaded successfully');
              console.log(listing);
            } else {
              this.isLoading = false;
              this.router.navigate(['../'], { relativeTo: this.route });
            }
          },
          error: (error) => {
            console.error('Error updating listing', error);
            this.formErrors['submit'] = 'Failed to update listing';
            this.isLoading = false;
          }
        })
      );

      this.updateListingStatus(this.listingId);
    }
  }

  private updateListingStatus(listingId: string): void {
    this.subscriptions.add(
      this.listingsService.updateListingStatus(listingId, {verificationStatusId: 2}).subscribe({
        next: () => {
          console.log('Listing status updated to PENDING');
        },
        error: (error) => {
          console.error('Error updating listing status', error.error.message);
          this.formErrors['submit'] = 'Failed to update listing status';
          this.isLoading = false;
        }
      })
    );
  }

  private uploadImages(listingId: string): void {
    const uploads = this.uploadedImages.map(img => {
      console.log('Uploading image:', img.file, img.preview)
      return this.listingsService.uploadListingPhoto(listingId, img.file)
    });

    this.subscriptions.add(
      forkJoin(uploads).subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(['../'], { relativeTo: this.route });
        },
        error: (error) => {
          console.error('Error uploading images', error);
          this.formErrors['submit'] = 'Failed to upload images';
          this.isLoading = false;
        }
      })
    );
  }

  markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
