﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Amenity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid CategoryId { get; set; }

    public string Icon { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual AmenityCategory Category { get; set; }

    public virtual ICollection<ListingAmenity> ListingAmenities { get; set; } = new List<ListingAmenity>();
}