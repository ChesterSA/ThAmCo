﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Data.Migrations
{
    [DbContext(typeof(EventsDbContext))]
    [Migration("20181202161956_CostIsDecimal")]
    partial class CostIsDecimal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("thamco.events")
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThAmCo.Events.Data.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new { Id = 1, Email = "bob@example.com", FirstName = "Robert", Surname = "Robertson" },
                        new { Id = 2, Email = "betty@example.com", FirstName = "Betty", Surname = "Thornton" },
                        new { Id = 3, Email = "jin@example.com", FirstName = "Jin", Surname = "Jellybeans" },
                        new { Id = 4, Email = "guy@example.com", FirstName = "Guy", Surname = "Johnson" },
                        new { Id = 5, Email = "stacey@example.com", FirstName = "Stacey", Surname = "Noble" },
                        new { Id = 6, Email = "martha@example.com", FirstName = "Martha", Surname = "Simons" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<TimeSpan?>("Duration");

                    b.Property<decimal>("FoodCost");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Menu");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("TypeId")
                        .IsRequired()
                        .IsFixedLength(true)
                        .HasMaxLength(3);

                    b.Property<string>("Venue");

                    b.Property<decimal>("VenueCost");

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasData(
                        new { Id = 1, Date = new DateTime(2016, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), Duration = new TimeSpan(0, 6, 0, 0, 0), FoodCost = 0m, IsActive = true, Title = "Bob's Big 50", TypeId = "PTY", VenueCost = 0m },
                        new { Id = 2, Date = new DateTime(2018, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Duration = new TimeSpan(0, 12, 0, 0, 0), FoodCost = 0m, IsActive = true, Title = "Best Wedding Yet", TypeId = "WED", VenueCost = 0m },
                        new { Id = 3, Date = new DateTime(2018, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), Duration = new TimeSpan(0, 1, 0, 0, 0), FoodCost = 0m, IsActive = true, Title = "Best-er Wedding Yet", TypeId = "WED", VenueCost = 0m }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("EventId");

                    b.Property<bool>("Attended");

                    b.HasKey("CustomerId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("Guests");

                    b.HasData(
                        new { CustomerId = 1, EventId = 1, Attended = true },
                        new { CustomerId = 2, EventId = 1, Attended = false },
                        new { CustomerId = 1, EventId = 2, Attended = false },
                        new { CustomerId = 3, EventId = 2, Attended = false }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("FirstAider");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("StaffCode")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Staff");

                    b.HasData(
                        new { Id = 1, FirstAider = true, FirstName = "Sam", StaffCode = "SS1", Surname = "Shaw" },
                        new { Id = 2, FirstAider = false, FirstName = "Andrew", StaffCode = "AM2", Surname = "Martin" },
                        new { Id = 3, FirstAider = false, FirstName = "Jeremy", StaffCode = "JO3", Surname = "Usbourne" },
                        new { Id = 4, FirstAider = true, FirstName = "Kyle", StaffCode = "KK4", Surname = "Kelly" },
                        new { Id = 5, FirstAider = false, FirstName = "Simon", StaffCode = "SB5", Surname = "Belmont" },
                        new { Id = 6, FirstAider = false, FirstName = "Harry", StaffCode = "HS6", Surname = "Smith" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staffing", b =>
                {
                    b.Property<int>("StaffId");

                    b.Property<int>("EventId");

                    b.HasKey("StaffId", "EventId");

                    b.HasAlternateKey("EventId", "StaffId");

                    b.ToTable("Workers");

                    b.HasData(
                        new { StaffId = 1, EventId = 1 },
                        new { StaffId = 2, EventId = 1 },
                        new { StaffId = 1, EventId = 2 },
                        new { StaffId = 3, EventId = 2 }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Models.FoodMenuViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Cost");

                    b.Property<string>("Dessert");

                    b.Property<string>("Main")
                        .IsRequired();

                    b.Property<string>("Starter");

                    b.HasKey("Id");

                    b.ToTable("FoodMenuViewModel");
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.HasOne("ThAmCo.Events.Data.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ThAmCo.Events.Data.Event", "Event")
                        .WithMany("Bookings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staffing", b =>
                {
                    b.HasOne("ThAmCo.Events.Data.Event", "Event")
                        .WithMany("Staff")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ThAmCo.Events.Data.Staff", "Staff")
                        .WithMany("Jobs")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
