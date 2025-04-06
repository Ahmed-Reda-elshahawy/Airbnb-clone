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
    internal class User_Configurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(U => U.Email)
                .IsUnique();

            builder.Property(U => U.Email)
                .IsRequired();

            builder.Property(U => U.Password)
                .IsRequired();

            builder.Property(U => U.First_Name)
                   .IsRequired();

            builder.Property(U => U.Last_Nmae)
                   .IsRequired();




        }
    }
}
