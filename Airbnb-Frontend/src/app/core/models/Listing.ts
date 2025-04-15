export interface Listing {
  id: string;
  hostId: string;
  title: string;
  description: string;
  propertyTypeId: number;
  roomTypeId: number;
  capacity: number;
  bedrooms: number;
  bathrooms: number;
  pricePerNight: number;
  serviceFee: number;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: "available" | "not available";
  country: string;
  postalCode: string;
  latitude: number;
  longitude: number;
  instantBooking: boolean;
  createdAt: Date;
  updatedAt: Date;
  minNights: number;
  maxNights: number;
  cancellationPolicyId: number;
  averageRating: number;
  reviewCount: number;
  isActive: boolean;
  currencyId: number;
  imageUrls: string[];
  previewImageUrl: string;
  amenities:{
    id:string;
    name:string;
    categoryId:string;
    icon:string;
    createdAt:Date
  }[];

}
