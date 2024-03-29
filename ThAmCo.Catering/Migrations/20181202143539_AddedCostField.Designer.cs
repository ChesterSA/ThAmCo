﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Migrations
{
    [DbContext(typeof(CateringDbContext))]
    [Migration("20181202143539_AddedCostField")]
    partial class AddedCostField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("thamco.catering")
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThAmCo.Catering.Data.Booking", b =>
                {
                    b.Property<int>("MenuId");

                    b.Property<int>("EventId");

                    b.HasKey("MenuId", "EventId");

                    b.HasAlternateKey("EventId", "MenuId");

                    b.ToTable("Booking");

                    b.HasData(
                        new { MenuId = 1, EventId = 1 },
                        new { MenuId = 2, EventId = 2 }
                    );
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Cost");

                    b.Property<string>("Dessert");

                    b.Property<string>("Main")
                        .IsRequired();

                    b.Property<string>("Starter");

                    b.HasKey("Id");

                    b.ToTable("Menus");

                    b.HasData(
                        new { Id = 1, Cost = 10.4, Dessert = "Ice Cream", Main = "Roast Dinner", Starter = "Prawn Cocktail" },
                        new { Id = 2, Cost = 15.5, Dessert = "Chocolate Fudge Cake", Main = "Burger & Chips", Starter = "Soup" },
                        new { Id = 3, Cost = 12.3, Dessert = "Apple Pie", Main = "Chilli Con Carne", Starter = "Onion Rings" },
                        new { Id = 4, Cost = 19.95, Dessert = "Jam Roly-Poly", Main = "Spaghetti Bolognese", Starter = "Nachos" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.Booking", b =>
                {
                    b.HasOne("ThAmCo.Catering.Data.FoodMenu", "Menu")
                        .WithMany("Bookings")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
