using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Data
{
    public class EventsDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<GuestBooking> Guests { get; set; }
        public DbSet<Staffing> Workers { get; set; }
        public DbSet<Staff> Staff { get; set; }

        private IHostingEnvironment HostEnv { get; }

        public EventsDbContext(DbContextOptions<EventsDbContext> options,
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

            builder.HasDefaultSchema("thamco.events");

            builder.Entity<GuestBooking>()
                   .HasKey(b => new { b.CustomerId, b.EventId });

            builder.Entity<Staffing>()
                   .HasKey(b => new { b.StaffId, b.EventId });

            builder.Entity<Staff>()
                  .HasMany(c => c.Jobs)
                  .WithOne(b => b.Staff)
                  .HasForeignKey(b => b.StaffId);

            builder.Entity<Customer>()
                   .HasMany(c => c.Bookings)
                   .WithOne(b => b.Customer)
                   .HasForeignKey(b => b.CustomerId);

            builder.Entity<Event>()
                   .HasMany(e => e.Bookings)
                   .WithOne(b => b.Event)
                   .HasForeignKey(b => b.EventId);

            builder.Entity<Event>()
                   .Property(e => e.TypeId)
                   .IsFixedLength();

            // seed data for debug / development testing
            if (HostEnv != null && HostEnv.IsDevelopment())
            {
                builder.Entity<Customer>().HasData(
                    new Customer { Id = 1, Surname = "Robertson", FirstName = "Robert", Email = "bob@example.com" },
                    new Customer { Id = 2, Surname = "Thornton", FirstName = "Betty", Email = "betty@example.com" },
                    new Customer { Id = 3, Surname = "Jellybeans", FirstName = "Jin", Email = "jin@example.com" },
                    new Customer { Id = 4, Surname = "Johnson", FirstName = "Guy", Email = "guy@example.com" },
                    new Customer { Id = 5, Surname = "Noble", FirstName = "Stacey", Email = "stacey@example.com" },
                    new Customer { Id = 6, Surname = "Simons", FirstName = "Martha", Email = "martha@example.com" }
                );

                builder.Entity<Staff>().HasData(
                    new Staff { Id = 1, StaffCode = "SS1", Surname = "Shaw", FirstName = "Sam", FirstAider = true },
                    new Staff { Id = 2, StaffCode = "AM2", Surname = "Martin", FirstName = "Andrew", FirstAider = false },
                    new Staff { Id = 3, StaffCode = "JO3", Surname = "Usbourne", FirstName = "Jeremy", FirstAider = false },
                    new Staff { Id = 4, StaffCode = "KK4", Surname = "Kelly", FirstName = "Kyle", FirstAider = true },
                    new Staff { Id = 5, StaffCode = "SB5", Surname = "Belmont", FirstName = "Simon", FirstAider = false },
                    new Staff { Id = 6, StaffCode = "HS6", Surname = "Smith", FirstName = "Harry", FirstAider = false }
                );

                builder.Entity<Event>().HasData(
                    new Event { Id = 1, IsActive  = true, Title = "Bob's Big 50", Date = new DateTime(2016, 4, 12), Duration = new TimeSpan(6, 0, 0), TypeId = "PTY" },
                    new Event { Id = 2, IsActive = true,  Title = "Best Wedding Yet", Date = new DateTime(2018, 12, 1), Duration = new TimeSpan(12, 0, 0), TypeId = "WED" },
                    new Event { Id = 3, IsActive = true, Title = "Best-er Wedding Yet", Date = new DateTime(2018, 10, 28), Duration = new TimeSpan(1, 0, 0), TypeId = "WED" }
                );

                builder.Entity<GuestBooking>().HasData(
                    new GuestBooking { CustomerId = 1, EventId = 1, Attended = true },
                    new GuestBooking { CustomerId = 2, EventId = 1, Attended = false },
                    new GuestBooking { CustomerId = 1, EventId = 2, Attended = false },
                    new GuestBooking { CustomerId = 3, EventId = 2, Attended = false }
                );

                builder.Entity<Staffing>().HasData(
                    new Staffing { StaffId = 1, EventId = 1 },
                    new Staffing { StaffId = 2, EventId = 1},
                    new Staffing { StaffId = 1, EventId = 2 },
                    new Staffing { StaffId = 3, EventId = 2 }
                );
            }
        }
    }
}
