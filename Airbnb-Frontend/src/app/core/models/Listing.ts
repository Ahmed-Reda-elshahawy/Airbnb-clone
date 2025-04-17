export interface Listing {
  id: string;
  host: {
    id: string;
    firstName: string;
    lastName: string;
    profilePictureUrl: string;
    bio: string;
    isHost: boolean;
    isVerified: boolean;
  };
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
  state: string;
  country: string;
  postalCode: string;
  latitude: number;
  longitude: number;
  instantBooking: boolean;
  createdAt: string;
  updatedAt: string;
  minNights: number;
  maxNights: number;
  cancellationPolicyId: number;
  averageRating: number;
  reviewCount: number;
  isActive: boolean;
  currencyId: number;
  imageUrls: string[];
  previewImageUrl: string;
  amenities: {
    id: string;
    name: string;
    categoryId: string;
    icon: string;
    createdAt: string;
  }[];
  reviews: {
    id: string;
    bookingId: string;
    reviewer: {
      id: string;
      firstName: string;
      lastName: string;
      profilePictureUrl: string;
      bio: string;
      isHost: boolean;
      isVerified: boolean;
    };
    hostId: string;
    listingId: string;
    comment: string;
    hostReply: string;
    hostReplyDate: string;
    createdAt: string;
    updatedAt: string;
    rating: number;
    cleanlinessRating: number;
    accuracyRating: number;
    communicationRating: number;
    locationRating: number;
    checkInRating: number;
    valueRating: number;
  }[];

}
