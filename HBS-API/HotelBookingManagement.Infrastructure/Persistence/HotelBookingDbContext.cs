using HotelBookingManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingManagement.Infrastructure.Persistence
{
    public class HotelBookingDbContext : DbContext
    {
        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options)
            : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomAvailability> RoomAvailabilities { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingRoom> BookingRooms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations here if using Fluent API
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Name).IsRequired().HasMaxLength(200);
                entity.Property(h => h.Address).IsRequired().HasMaxLength(500);
                entity.Property(h => h.City).IsRequired().HasMaxLength(100);
                entity.Property(h => h.Country).IsRequired().HasMaxLength(100);
                entity.Property(h => h.Email).HasMaxLength(200);
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Name).IsRequired().HasMaxLength(100);
                entity.Property(rt => rt.MaxGuests).IsRequired();
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.RoomNumber).IsRequired().HasMaxLength(50);
                entity.Property(r => r.PricePerNight).HasColumnType("decimal(18,2)");
                
                entity.HasOne(r => r.Hotel)
                      .WithMany(h => h.Rooms)
                      .HasForeignKey(r => r.HotelId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.RoomType)
                      .WithMany()
                      .HasForeignKey(r => r.RoomTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RoomImage>(entity =>
            {
                entity.HasKey(ri => ri.Id);
                entity.Property(ri => ri.ImageUrl).IsRequired().HasMaxLength(500);

                entity.HasOne(ri => ri.Room)
                      .WithMany(r => r.Images)
                      .HasForeignKey(ri => ri.RoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RoomAvailability>(entity =>
            {
                entity.HasKey(ra => ra.Id);
                entity.Property(ra => ra.PriceOverride).HasColumnType("decimal(18,2)");

                entity.HasOne(ra => ra.Room)
                      .WithMany(r => r.Availabilities)
                      .HasForeignKey(ra => ra.RoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.FullName).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Phone).IsRequired().HasMaxLength(50);
                entity.HasIndex(c => c.Email).IsUnique();
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");

                entity.HasOne(b => b.Customer)
                      .WithMany(c => c.Bookings)
                      .HasForeignKey(b => b.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Hotel)
                      .WithMany()
                      .HasForeignKey(b => b.HotelId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BookingRoom>(entity =>
            {
                entity.HasKey(br => br.Id);
                entity.Property(br => br.PricePerNight).HasColumnType("decimal(18,2)");

                entity.HasOne(br => br.Booking)
                      .WithMany(b => b.BookingRooms)
                      .HasForeignKey(br => br.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(br => br.Room)
                      .WithMany()
                      .HasForeignKey(br => br.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                entity.Property(p => p.TransactionId).HasMaxLength(200);

                entity.HasOne(p => p.Booking)
                      .WithMany()
                      .HasForeignKey(p => p.BookingId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasKey(al => al.Id);
                entity.Property(al => al.Description).HasMaxLength(1000);
            });
        }
    }
}
