﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class AvailabilityCalendar
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public DateTime Date { get; set; }
    public bool? IsAvailable { get; set; }
    public decimal? SpecialPrice { get; set; }
    public virtual Listing Listing { get; set; }
}