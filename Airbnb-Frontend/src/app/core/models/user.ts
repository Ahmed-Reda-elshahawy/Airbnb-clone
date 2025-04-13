export interface Country {
  name?: string;
  code?: string;
}

export interface Representative {
  name?: string;
  image?: string;
}

export interface User {
  id: string,
  firstName: string,
  lastName: string,
  dateOfBirth: Date,
  profilePictureUrl: string,
  createdAt: Date,
  updatedAt: Date,
  bio: string,
  isHost: boolean,
  isVerified: boolean,
  verificationStatusId: number,
  isAdmin: boolean,
  lastLogin: Date,
  preferredLanguage: string,
  currencyId: number,
  bookings: any[],
  currency: null,
  listings: any[],
  messageRecipients: any[],
  messageSenders: any[],
  payments: any[],
  reviewHosts: any[],
  reviewReviewers: any[],
  verificationStatus: null,
  wishlists: any[]
}
