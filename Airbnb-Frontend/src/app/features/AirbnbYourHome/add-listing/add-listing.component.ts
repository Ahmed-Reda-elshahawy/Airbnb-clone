import { CommonModule, Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ListingsService } from '../../../core/services/listings.service';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-add-listing',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-listing.component.html',
  styleUrl: './add-listing.component.css'
})
export class AddListingComponent implements OnInit, OnDestroy {
  private subscriptions = new Subscription();
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private listingsService: ListingsService
  ) { }
  // Main form group that contains all steps
  listingForm: FormGroup = new FormGroup({});
  // Listing ID from URL
  listingId!: string;
  // Current step tracking
  currentStep!: string;
  formProgress: number = 0;
  // Step configuration
  steps = [
    { id: 'property-type', name: 'Property Type', progress: 0 },
    { id: 'location', name: 'Location', progress: 20 },
    { id: 'structure', name: 'Structure', progress: 40 },
    { id: 'amenities', name: 'Amenities', progress: 60 },
    { id: 'photos', name: 'Photos', progress: 80 },
    { id: 'price', name: 'Price', progress: 90 },
    { id: 'review', name: 'Review & Publish', progress: 100 }
  ];
  // Form data options
  propertyTypes = [
    { id: 'apartment', name: 'Apartment', icon: 'apartment' },
    { id: 'house', name: 'House', icon: 'home' },
    { id: 'secondary', name: 'Secondary unit', icon: 'foundation' },
    { id: 'unique', name: 'Unique space', icon: 'deck' },
    { id: 'bed-breakfast', name: 'Bed and breakfast', icon: 'hotel' },
    { id: 'boutique', name: 'Boutique hotel', icon: 'corporate_fare' }
  ];
  amenities = [
    { id: 'wifi', name: 'Wifi', icon: 'wifi' },
    { id: 'tv', name: 'TV', icon: 'tv' },
    { id: 'kitchen', name: 'Kitchen', icon: 'kitchen' },
    { id: 'washer', name: 'Washer', icon: 'local_laundry_service' },
    { id: 'parking', name: 'Free parking', icon: 'local_parking' },
    { id: 'ac', name: 'Air conditioning', icon: 'ac_unit' },
    { id: 'workspace', name: 'Workspace', icon: 'desk' },
    { id: 'pool', name: 'Pool', icon: 'pool' }
  ];

  // Form state management
  saving = false;
  formErrors: any = {};
  formChanged = false;



  ngOnInit(): void {
    // Initialize the form
    this.initForm();

    // Get listing ID and current step from URL
    this.subscriptions.add(
      this.route.params.subscribe(params => {
        this.listingId = params['id'];
        this.currentStep = params['step'] || 'property-type';

        // Update progress based on current step
        this.updateProgress();

        // Load existing data if available
        this.loadListingData();
      })
    );

    // Track form changes
    this.subscriptions.add(
      this.listingForm.valueChanges.subscribe(() => {
        this.formChanged = true;
        // Auto-save after 3 seconds of inactivity
        this.debouncedSave();
      })
    );
  }

  initForm(): void {
    this.listingForm = this.fb.group({
      // Step 1: Property Type
      propertyType: ['', Validators.required],

      // Step 2: Location
      location: this.fb.group({
        country: ['', Validators.required],
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipCode: ['', Validators.required],
        lat: [null],
        lng: [null]
      }),

      // Step 3: Structure
      structure: this.fb.group({
        guests: [1, [Validators.required, Validators.min(1)]],
        bedrooms: [1, [Validators.required, Validators.min(0)]],
        beds: [1, [Validators.required, Validators.min(1)]],
        bathrooms: [1, [Validators.required, Validators.min(0.5)]]
      }),

      // Step 4: Amenities
      amenities: this.fb.array([]),

      // Step 5: Photos & Description
      photos: this.fb.array([]),
      title: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.required, Validators.minLength(50), Validators.maxLength(500)]],

      // Step 6: Price
      price: this.fb.group({
        basePrice: [50, [Validators.required, Validators.min(10)]],
        cleaningFee: [0, [Validators.min(0)]],
        serviceFee: [{ value: 0, disabled: true }],
        smartPricing: [false]
      })
    });
  }

  // Debounce helper for auto-save
  private saveTimeout: any;
  debouncedSave(): void {
    clearTimeout(this.saveTimeout);
    this.saveTimeout = setTimeout(() => {
      if (this.formChanged) {
        this.saveProgress(false);
      }
    }, 3000);
  }

  loadListingData(): void {
    this.subscriptions.add(
      this.listingsService.getDraftListing(this.listingId).pipe(
        take(1)
      ).subscribe({
        next: (data) => {
          if (data) {
            // Patch form values from retrieved data
            this.listingForm.patchValue(data);

            // Handle form arrays separately
            if (data.amenities?.length) {
              this.patchFormArray('amenities', data.amenities);
            }

            if (data.imageUrls?.length) {
              this.patchFormArray('photos', data.imageUrls);
            }

            this.formChanged = false;
          }
        },
        error: (error) => {
          console.error('Error loading listing data', error);
          this.formErrors['general'] = 'Failed to load listing data';
          // Optionally redirect to create new listing
          // this.router.navigate(['/become-a-host']);
        }
      })
    );
  }

  patchFormArray(arrayName: string, values: any[]): void {
    const formArray = this.listingForm.get(arrayName) as FormArray;
    formArray.clear();

    values.forEach(value => {
      formArray.push(this.fb.control(value));
    });
  }

  updateProgress(): void {
    const currentStepIndex = this.steps.findIndex(step => step.id === this.currentStep);
    if (currentStepIndex !== -1) {
      this.formProgress = this.steps[currentStepIndex].progress;
    }
  }

  // Navigation methods
  goToStep(stepId: string): void {
    // Validate current step before proceeding
    if (this.isCurrentStepValid() || stepId === this.getPreviousStepId()) {
      // Save progress
      this.saveProgress(true, () => {
        // Navigate to new step
        this.router.navigate(['/become-a-host', this.listingId, stepId]);
      });
    } else {
      this.markCurrentStepAsTouched();
    }
  }

  nextStep(): void {
    const currentIndex = this.steps.findIndex(step => step.id === this.currentStep);
    if (currentIndex < this.steps.length - 1) {
      this.goToStep(this.steps[currentIndex + 1].id);
    }
  }

  prevStep(): void {
    const currentIndex = this.steps.findIndex(step => step.id === this.currentStep);
    if (currentIndex > 0) {
      this.goToStep(this.steps[currentIndex - 1].id);
    }
  }

  getPreviousStepId(): string {
    const currentIndex = this.steps.findIndex(step => step.id === this.currentStep);
    return currentIndex > 0 ? this.steps[currentIndex - 1].id : '';
  }

  // Form validation
  isCurrentStepValid(): boolean {
    switch (this.currentStep) {
      case 'property-type':
        const propertyType = this.listingForm.get('propertyType');
        return propertyType ? propertyType.valid : false;
      case 'location':
        const locationGroup = this.listingForm.get('location');
        return locationGroup ? locationGroup.valid : false;
      case 'structure':
        const structureGroup = this.listingForm.get('structure');
        return structureGroup ? structureGroup.valid : false;
      case 'amenities':
        return true; // Amenities are optional
      case 'photos':
        const photos = this.listingForm.get('photos') as FormArray;
        return photos.length > 0 &&
          this.listingForm.get('title')?.valid &&
          this.listingForm.get('description')?.valid ? true : false;
      case 'price':
        return this.listingForm.get('price')?.valid ? true : false;
      case 'review':
        return this.listingForm.valid;
      default:
        return true;
    }
  }

  markCurrentStepAsTouched(): void {
    switch (this.currentStep) {
      case 'property-type':
        this.listingForm.get('propertyType')?.markAsTouched();
        break;
      case 'location':
        Object.keys(this.listingForm.get('location')?.value).forEach(key => {
          this.listingForm.get(`location.${key}`)?.markAsTouched();
        });
        break;
      case 'structure':
        Object.keys(this.listingForm.get('structure')?.value).forEach(key => {
          this.listingForm.get(`structure.${key}`)?.markAsTouched();
        });
        break;
      case 'photos':
        this.listingForm.get('title')?.markAsTouched();
        this.listingForm.get('description')?.markAsTouched();
        break;
      case 'price':
        this.listingForm.get('price.basePrice')?.markAsTouched();
        break;
    }
  }

  // Save form data
  saveProgress(showLoading: boolean = true, callback?: Function): void {
    if (this.saving) return;

    if (showLoading) {
      this.saving = true;
    }

    const formData = this.prepareFormData();

    this.subscriptions.add(
      this.listingsService.updateDraftListing(this.listingId, formData).pipe(
        take(1)
      ).subscribe({
        next: (response) => {
          this.saving = false;
          this.formChanged = false;
          this.formErrors = {};

          if (callback) {
            callback();
          }
        },
        error: (error) => {
          this.saving = false;
          console.error('Error saving listing data', error);
          this.formErrors = error.errors || { general: 'Failed to save listing data' };
        }
      })
    );
  }

  prepareFormData(): any {
    // Clone the form value to avoid modifying the original
    const formData = { ...this.listingForm.value };

    // Add any required transformations here
    // For example, handling FormArray data, etc.

    return formData;
  }

  // Form interaction methods
  selectPropertyType(typeId: string): void {
    this.listingForm.patchValue({
      propertyType: typeId
    });
  }

  toggleAmenity(amenityId: string): void {
    const amenities = this.listingForm.get('amenities') as FormArray;
    const index = amenities.value.indexOf(amenityId);

    if (index === -1) {
      amenities.push(this.fb.control(amenityId));
    } else {
      amenities.removeAt(index);
    }
  }

  isAmenitySelected(amenityId: string): boolean {
    const amenities = this.listingForm.get('amenities') as FormArray;
    return amenities.value.includes(amenityId);
  }

  // File upload handling
  onPhotoUpload(event: any): void {
    const files = event.target.files;
    if (files) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        this.uploadPhoto(file);
      }
    }
  }

  uploadPhoto(file: File): void {
    if (!file) return;

    this.subscriptions.add(
      this.listingsService.uploadListingPhoto(this.listingId, file).pipe(
        take(1)
      ).subscribe({
        next: (response) => {
          const photos = this.listingForm.get('photos') as FormArray;
          photos.push(this.fb.control(response.photoUrl));
          this.formChanged = true;
        },
        error: (error) => {
          console.error('Error uploading photo', error);
          this.formErrors['photos'] = 'Failed to upload photo';
        }
      })
    );
  }

  removePhoto(index: number): void {
    const photos = this.listingForm.get('photos') as FormArray;
    photos.removeAt(index);
  }

  // Submission
  publishListing(): void {
    if (this.listingForm.valid) {
      this.saving = true;
      this.subscriptions.add(
        this.listingsService.publishListing(this.listingId).pipe(
          take(1)
        ).subscribe({
          next: (response) => {
            this.saving = false;
            // Redirect to success page or listing page
            this.router.navigate(['/listing', this.listingId]);
          },
          error: (error) => {
            this.saving = false;
            console.error('Error publishing listing', error);
            this.formErrors = error.errors || { general: 'Failed to publish listing' };
          }
        })
      );
    } else {
      // Mark all fields as touched to show validation errors
      this.markFormGroupTouched(this.listingForm);
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
    clearTimeout(this.saveTimeout);
  }

  markFormGroupTouched(formGroup: FormGroup | FormArray): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);

      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markFormGroupTouched(control);
      } else {
        control?.markAsTouched();
      }
    });
  }

  get photosLength(): number {
    const photos = this.listingForm.get('photos') as FormArray;
    return photos ? photos.length : 0;
  }

}
