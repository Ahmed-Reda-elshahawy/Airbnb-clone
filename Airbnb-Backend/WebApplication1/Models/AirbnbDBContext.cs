﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.ChatBot;
using WebApplication1.Models.Enums;

namespace WebApplication1.Models;

public partial class AirbnbDBContext : WebApplication1Context
{
    public AirbnbDBContext(DbContextOptions<AirbnbDBContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Amenity> Amenities { get; set; }
    public virtual DbSet<AmenityCategory> AmenityCategory { get; set; }
    public virtual DbSet<AvailabilityCalendar> AvailabilityCalendars { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<CancellationPolicy> CancellationPolicies { get; set; }
    public virtual DbSet<Currency> Currencies { get; set; }
    public virtual DbSet<Listing> Listings { get; set; }
    public virtual DbSet<ListingAmenity> ListingAmenities { get; set; }
    public virtual DbSet<ListingPhoto> ListingPhotos { get; set; }
    public virtual DbSet<AdditionalInformation> AdditionalInformation { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<ApplicationUser> Users { get; set; }

    public virtual DbSet<VerificationStatus> VerificationStatuses { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    public virtual DbSet<WishlistItem> WishlistItems { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Conversation> Conversations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Amenitie__3214EC07787697E8");

            entity.HasIndex(e => e.Name, "UQ__Amenitie__72E12F1B86B281BB").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CategoryId)
                .IsRequired()
                .HasColumnName("categoryId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.HasOne(e => e.Category)
                .WithMany(p => p.Amenities)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Amenities__categ__6A30C649");
        });

        modelBuilder.Entity<AmenityCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AmenityCategory");

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Name)
                  .IsRequired() 
                  .HasMaxLength(100)
                  .IsUnicode(false) 
                  .HasColumnName("name");  

            entity.HasMany(e => e.Amenities)
                  .WithOne(a => a.Category)  
                  .HasForeignKey(a => a.CategoryId)  
                  .OnDelete(DeleteBehavior.Cascade);  
        });
        modelBuilder.Entity<AvailabilityCalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availabi__3214EC076FC3A8E2");

            entity.ToTable("AvailabilityCalendar");

            entity.HasIndex(e => new { e.ListingId, e.Date }, "UX_Calendar").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("isAvailable");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.SpecialPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("specialPrice");

            entity.HasOne(d => d.Listing).WithMany(p => p.AvailabilityCalendars)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("FK__Availabil__listi__07C12930");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__3214EC07AB24997F");

            entity.HasIndex(e => new { e.ListingId, e.CheckInDate, e.CheckOutDate }, "IX_Bookings_DateRange");

            entity.HasIndex(e => e.GuestId, "IX_Bookings_GuestId");

            entity.HasIndex(e => e.Status, "IX_Bookings_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("bookingDate");
            entity.Property(e => e.CancellationReason)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("cancellationReason");
            entity.Property(e => e.CheckInDate)
                .HasColumnType("date")
                .HasColumnName("checkInDate");
            entity.Property(e => e.CheckOutDate)
                .HasColumnType("date")
                .HasColumnName("checkOutDate");
            entity.Property(e => e.GuestId).HasColumnName("guestId");
            entity.Property(e => e.GuestsCount).HasColumnName("guestsCount");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.SpecialRequests)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("specialRequests");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(BookingStatus.Pending)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Guest).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__guestI__114A936A");

            entity.HasOne(d => d.Listing).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("FK__Bookings__listin__123EB7A3");
        });

        modelBuilder.Entity<CancellationPolicy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cancella__3214EC07F9731500");

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currenci__3214EC07C2D97148");

            entity.HasIndex(e => e.Code, "UQ__Currenci__357D4CF90BB6C97F").IsUnique();

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Symbol)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("symbol");
        });

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Listings__3214EC0736988D9A");

            entity.HasIndex(e => e.Capacity, "IX_Listings_Capacity");

            entity.HasIndex(e => e.HostId, "IX_Listings_HostId");

            entity.HasIndex(e => new { e.City, e.State, e.Country }, "IX_Listings_Location");

            entity.HasIndex(e => e.PricePerNight, "IX_Listings_Price");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("addressLine1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("addressLine2");
            entity.Property(e => e.AverageRating)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("averageRating");
            entity.Property(e => e.Bathrooms).HasColumnName("bathrooms");
            entity.Property(e => e.Bedrooms).HasColumnName("bedrooms");
            entity.Property(e => e.CancellationPolicyId).HasColumnName("cancellationPolicyId");
            entity.Property(e => e.Capacity)
                .HasDefaultValue(1)
                .IsRequired()
                .HasColumnName("capacity");
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("currencyId");
            entity.Property(e => e.SecurityDeposit)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("securityDeposit");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.HostId).HasColumnName("hostId");
            entity.Property(e => e.InstantBooking)
                .HasDefaultValue(false)
                .HasColumnName("instantBooking");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .IsRequired()
                .HasColumnName("isActive");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(10, 8)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(11, 8)")
                .HasColumnName("longitude");
            entity.Property(e => e.MaxNights).HasColumnName("maxNights");
            entity.Property(e => e.MinNights)
                .HasDefaultValue(1)
                .IsRequired()
                .HasColumnName("minNights");
            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("postalCode");
            entity.Property(e => e.PricePerNight)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("pricePerNight");
            entity.Property(e => e.PropertyTypeId).HasColumnName("propertyTypeId");
            entity.Property(e => e.ReviewCount)
                .HasDefaultValue(0)
                .HasColumnName("reviewCount");
            entity.Property(e => e.RoomTypeId).HasColumnName("roomTypeId");
            entity.Property(e => e.ServiceFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("serviceFee");
            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.VerificationStatusId).HasColumnName("verificationStatusId").HasDefaultValue(1);
            entity.HasOne(d => d.VerificationStatus).WithMany(p => p.Listings)
                .HasForeignKey(d => d.VerificationStatusId)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasConstraintName("FK__Listing__verificat__4BAC3F29");


            entity.HasOne(d => d.CancellationPolicy).WithMany(p => p.Listings)
                .HasForeignKey(d => d.CancellationPolicyId)
                .HasConstraintName("FK__Listings__cancel__5CD6CB2B");

            entity.HasOne(d => d.Currency).WithMany(p => p.Listings)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK__Listings__curren__5DCAEF64");

            entity.HasOne(d => d.Host).WithMany(p => p.Listings)
                .HasForeignKey(d => d.HostId)
                .HasConstraintName("FK__Listings__hostId__59FA5E80");

            entity.HasOne(d => d.PropertyType).WithMany(p => p.Listings)
                .HasForeignKey(d => d.PropertyTypeId)
                .HasConstraintName("FK__Listings__proper__5AEE82B9");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Listings)
                .HasForeignKey(d => d.RoomTypeId)
                .HasConstraintName("FK__Listings__roomTy__5BE2A6F2");
        });
        modelBuilder.Entity<AdditionalInformation>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Additio__3214EC07F0A1500F");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Data)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("data");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.Description)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("description");
            entity.HasOne(d => d.Listing).WithMany(p => p.AdditionalInformation)
                .HasForeignKey(d => d.ListingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Additiona__listi__7D439ABD");
        });
        modelBuilder.Entity<ListingAmenity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListingA__3214EC074C630FAB");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AmenityId).HasColumnName("amenityId");

            entity.HasOne(d => d.Amenity).WithMany(p => p.ListingAmenities)
                .HasForeignKey(d => d.AmenityId)
                .HasConstraintName("FK__ListingAm__ameni__6D0D32F4");

            entity.HasOne(d => d.Listing).WithMany(p => p.ListingAmenities)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("FK__ListingAm__Listi__6E01572D");
        });

        modelBuilder.Entity<ListingPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListingP__3214EC078C970EA8");

            entity.HasIndex(e => e.ListingId, "UX_Listing_Photos_Primary")
                .IsUnique()
                .HasFilter("([isPrimary]=(1))");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Caption)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("caption");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("displayOrder");
            entity.Property(e => e.IsPrimary)
                .HasDefaultValue(false)
                .HasColumnName("isPrimary");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("uploadedAt");
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url");
            entity.HasOne(d => d.Listing).WithMany(p => p.ListingPhotos)
            .HasForeignKey(d => d.ListingId)
            .HasConstraintName("FK_ListingPhotos_Listing");

        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC077DF3B4BD");

            entity.HasIndex(e => new { e.SenderId, e.RecipientId }, "IX_Messages_SenderRecipient");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Content)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("isRead");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.RecipientId).HasColumnName("recipientId");
            entity.Property(e => e.SenderId).HasColumnName("senderId");
            entity.Property(e => e.SentTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sentTime");

            entity.HasOne(d => d.Listing).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ListingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Messages__listin__75A278F5");

            entity.HasOne(d => d.Recipient).WithMany(p => p.MessageRecipients)
                .HasForeignKey(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__recipi__74AE54BC");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__sender__73BA3083");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC074C3E3A00");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("currencyId");
            entity.Property(e => e.FailureReason)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("failureReason");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("paymentDate");
            entity.Property(e => e.PaymentMethodId).HasColumnName("paymentMethodId");
            entity.Property(e => e.PaymentType)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasConversion<string>()
                .HasDefaultValue(PaymentType.AllNow)
                .HasColumnName("paymentType");
            entity.Property(e => e.ProccessedAt).HasColumnType("datetime");
            entity.Property(e => e.ReceiptUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("receiptUrl");
            entity.Property(e => e.Status)
               .IsRequired()
               .HasMaxLength(20)
               .IsUnicode(false)
               .HasConversion<string>()
               .HasDefaultValue(PaymentStatus.Pending)
               .HasColumnName("status");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("transactionId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__booking__1F98B2C1");

            entity.HasOne(d => d.Currency).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK__Payment__currenc__22751F6C");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__payment__2180FB33");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Payment__userId__208CD6FA");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC07982F688D");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Property__3214EC07238ECA57");

            entity.Property(e => e.PropertyTypeName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("propertyTypeName");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("icon");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC07F0A1500F");

            entity.HasIndex(e => e.ListingId, "IX_Reviews_ListingId");

            entity.HasIndex(e => new { e.BookingId, e.ReviewerId }, "UQ_ReviewBooking").IsUnique();

            entity.HasIndex(e => new { e.BookingId, e.ReviewerId }, "UX_ReviewBooking").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccuracyRating).HasColumnName("accuracyRating").HasColumnType("decimal(3,1)").IsRequired();
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CheckInRating).HasColumnName("checkInRating").HasColumnType("decimal(3,1)").IsRequired();
            entity.Property(e => e.CleanlinessRating).HasColumnName("cleanlinessRating").HasColumnType("decimal(3,1)").IsRequired();
            entity.Property(e => e.Comment)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.CommunicationRating).HasColumnName("communicationRating").HasColumnType("decimal(3,1)").IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.HostId).HasColumnName("hostId");
            entity.Property(e => e.HostReply)
                .IsUnicode(false)
                .HasColumnName("hostReply");
            entity.Property(e => e.HostReplyDate)
                .HasColumnType("datetime")
                .HasColumnName("hostReplyDate");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.LocationRating).HasColumnName("locationRating").HasColumnType("decimal(3,1)").IsRequired();
            entity.Property(e => e.Rating).HasColumnName("rating").HasColumnType("decimal(3,1)").IsRequired();
            entity.Property(e => e.ReviewerId).HasColumnName("reviewerId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.ValueRating).HasColumnName("valueRating").HasColumnType("decimal(3,1)").IsRequired();

            entity.HasOne(d => d.Booking).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__booking__2FCF1A8A");

            entity.HasOne(d => d.Host).WithMany(p => p.ReviewHosts)
                .HasForeignKey(d => d.HostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__hostId__31B762FC");

            entity.HasOne(d => d.Listing).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("FK__Reviews__listing__32AB8735");

            entity.HasOne(d => d.Reviewer).WithMany(p => p.ReviewReviewers)
                .HasForeignKey(d => d.ReviewerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__reviewe__30C33EC3");

            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Review_CleanlinessRating", "[cleanlinessRating] BETWEEN 0 AND 5");
                t.HasCheckConstraint("CK_Review_AccuracyRating", "[accuracyRating] BETWEEN 0 AND 5");
                t.HasCheckConstraint("CK_Review_CheckInRating", "[checkInRating] BETWEEN 0 AND 5");
                t.HasCheckConstraint("CK_Review_CommunicationRating", "[communicationRating] BETWEEN 0 AND 5");
                t.HasCheckConstraint("CK_Review_LocationRating", "[locationRating] BETWEEN 0 AND 5");
                t.HasCheckConstraint("CK_Review_ValueRating", "[valueRating] BETWEEN 0 AND 5");
                t.HasCheckConstraint("CK_Review_Rating", "[rating] BETWEEN 0 AND 5");

            });
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomType__3214EC0769FE7197");

            entity.Property(e => e.RoomTypeName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("roomTypeName");
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07CF81F701");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Users__4849DA01544ADDB8").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164CADE8B40").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Bio)
                .IsUnicode(false)
                .HasColumnName("bio");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("currencyId");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .HasColumnName("isAdmin");
            entity.Property(e => e.IsHost)
                .HasDefaultValue(false)
                .HasColumnName("isHost");
            entity.Property(e => e.IsVerified)
                .HasDefaultValue(false)
                .HasColumnName("isVerified");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("lastLogin");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("passwordHash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("en")
                .HasColumnName("preferredLanguage");
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("profilePictureURL");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.VerificationStatusId).HasColumnName("verificationStatusId");

            entity.HasOne(d => d.Currency).WithMany(p => p.Users)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK__Users__currencyI__4CA06362");

            entity.HasOne(d => d.VerificationStatus).WithMany(p => p.Users)
                .HasForeignKey(d => d.VerificationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__verificat__4BAC3F29");
        });

        modelBuilder.Entity<VerificationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Verifica__3214EC0713125C7D");

            entity.ToTable("VerificationStatus");

            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wishlist__3214EC075E6F8444");

            entity.ToTable("Wishlist");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsPublic)
                .HasDefaultValue(false)
                .HasColumnName("isPublic");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Wishlist__userId__7B5B524B");
        });

        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wishlist__3214EC07B48E1F30");

            entity.HasIndex(e => new { e.WishlistId, e.ListingId }, "IX_WishlistItems_Lookup");

            entity.HasIndex(e => new { e.WishlistId, e.ListingId }, "UQ_WishlistItem").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("addedAt");
            entity.Property(e => e.ListingId).HasColumnName("listingId");
            entity.Property(e => e.WishlistId).HasColumnName("wishlistId");

            entity.HasOne(d => d.Listing).WithMany(p => p.WishlistItems)
                .HasForeignKey(d => d.ListingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WishlistI__listi__02084FDA");

            entity.HasOne(d => d.Wishlist).WithMany(p => p.WishlistItems)
                .HasForeignKey(d => d.WishlistId)
                .HasConstraintName("FK__WishlistI__wishl__01142BA1");


        });
        modelBuilder.Entity<Conversation>()
        .HasMany(c => c.Messages)
        .WithOne()
        .HasForeignKey(m => m.ConversationId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatMessage>()
            .HasIndex(m => m.ConversationId);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
