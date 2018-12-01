using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
namespace ThAmCo.Events.Data
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

            // seed data for debug / development testing
            if (HostEnv != null && HostEnv.IsDevelopment())
            {
                builder.Entity<FoodMenu>().HasData(
                    new FoodMenu { Id = 1, Starter = "Prawn Cocktail", Main = "Roast Dinner", Dessert = "Ice Cream"},
                    new FoodMenu { Id = 1, Starter = "Soup", Main = "Burger & Chips", Dessert = "Chocolate Fudge Cake" },
                    new FoodMenu { Id = 1, Starter = "Onion Rings", Main = "Chilli Con Carne", Dessert = "Apple Pie" },
                    new FoodMenu { Id = 1, Starter = "Nachos", Main = "Spaghetti Bolognese", Dessert = "Jam Roly-Poly" }
                );
            }
        }
    }
}
