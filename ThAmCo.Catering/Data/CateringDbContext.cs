using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
namespace ThAmCo.Catering.Data
{
    /// <summary>
    /// The link to the database for the ThAmCo.Catering project
    /// </summary>
    public class CateringDbContext : DbContext
    {
        /// <summary>
        /// The list of menus
        /// </summary>
        public DbSet<FoodMenu> Menus { get; set; }

        /// <summary>
        /// The list of bookings
        /// </summary>
        public DbSet<Booking> Booking { get; set; }

        /// <summary>
        /// The host environment for the database
        /// </summary>
        private IHostingEnvironment HostEnv { get; }

        /// <summary>
        /// Creates a new DbContext
        /// </summary>
        /// <param name="options">The DbContextOptions for the database</param>
        /// <param name="env">The host environment of the database</param>
        public CateringDbContext(DbContextOptions<CateringDbContext> options,
                               IHostingEnvironment env) : base(options)
        {
            HostEnv = env;
        }

        /// <summary>
        /// Sets up the database as it is being configured
        /// </summary>
        /// <param name="builder">The builder used by the DbContext</param>
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        /// <summary>
        /// Called when the database is first initialised, sets default values
        /// </summary>
        /// <param name="builder">the bulder to be used</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("thamco.catering");

            builder.Entity<FoodMenu>()
                   .HasMany(c => c.Bookings)
                   .WithOne(b => b.Menu)
                   .HasForeignKey(b => b.MenuId);

            builder.Entity<Booking>()
                   .HasKey(b => new { b.MenuId, b.EventId });

            // seed data for debug / development testing
            if (HostEnv != null && HostEnv.IsDevelopment())
            {
                builder.Entity<FoodMenu>().HasData(
                    new FoodMenu { Id = 1, Starter = "Prawn Cocktail", Main = "Roast Dinner", Dessert = "Ice Cream", Cost = 10.40m },
                    new FoodMenu { Id = 2, Starter = "Soup", Main = "Burger & Chips", Dessert = "Chocolate Fudge Cake", Cost = 15.50m },
                    new FoodMenu { Id = 3, Starter = "Onion Rings", Main = "Chilli Con Carne", Dessert = "Apple Pie", Cost = 12.30m },
                    new FoodMenu { Id = 4, Starter = "Nachos", Main = "Spaghetti Bolognese", Dessert = "Jam Roly-Poly", Cost = 19.95m }
                );
            }

            builder.Entity<Booking>().HasData(
                    new Booking { MenuId = 1, EventId = 1 },
                    new Booking { MenuId = 2, EventId = 2 }
                );


        }

        
    }
}
