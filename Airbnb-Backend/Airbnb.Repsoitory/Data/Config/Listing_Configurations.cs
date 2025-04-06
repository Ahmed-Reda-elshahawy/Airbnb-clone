using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Airbnb.Repsoitory.Data.Config
{
    internal class Listing_Configurations : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.Property(L => L.Price_Per_Night)
                .HasColumnType("decimal(18,2)");

            builder.Property(L => L.Service_Fee)
                   .HasColumnType("decimal(18,2)");

            builder.Property(L => L.Average_Rating)
                   .HasColumnType("decimal(18,2)");

            builder.Property(L => L.Latitude)
                   .HasColumnType("decimal(9,6)");
            
            builder.Property(L => L.Longitude)
                   .HasColumnType("decimal(9,6)");


            builder.HasOne(L => L.Host)
                .WithMany(u => u.Listings)
                .HasForeignKey(L => L.Host_Id);
                
                
                    


        }
    }
}
