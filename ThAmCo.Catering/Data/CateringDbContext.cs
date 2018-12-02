using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
namespace ThAmCo.Catering.Data
{
    public class CateringDbContext : DbContext
    {
        public DbSet<FoodMenu> Menus { get; set; }

        private IHostingEnvironment HostEnv { get; }

        public CateringDbContext(DbContextOptions<CateringDbContext> options,
                               IHostingEnvironment env) : base(options)
        {
            HostEnv = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

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
                    new FoodMenu { Id = 1, Starter = "Prawn Cocktail", Main = "Roast Dinner", Dessert = "Ice Cream", Cost = 10.40},
                    new FoodMenu { Id = 2, Starter = "Soup", Main = "Burger & Chips", Dessert = "Chocolate Fudge Cake", Cost = 15.50},
                    new FoodMenu { Id = 3, Starter = "Onion Rings", Main = "Chilli Con Carne", Dessert = "Apple Pie", Cost = 12.30 },
                    new FoodMenu { Id = 4, Starter = "Nachos", Main = "Spaghetti Bolognese", Dessert = "Jam Roly-Poly", Cost = 19.95 }
                );
            }

            builder.Entity<Booking>().HasData(
                    new Booking { MenuId = 1, EventId = 1 },
                    new Booking { MenuId = 2, EventId = 2 }
                );


        }

        public DbSet<ThAmCo.Catering.Data.Booking> Booking { get; set; }
    }
}
