using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;


namespace Airbnb.Repsoitory.Data
{
    public class Airbnb_Context : DbContext
    {


        public Airbnb_Context(DbContextOptions<Airbnb_Context> options ) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
