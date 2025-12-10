using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingDeviceLib.Models;

namespace TrackingDeviceLib.Data;

public class TrackingDeviceContext : DbContext
{    
    public TrackingDeviceContext(DbContextOptions<TrackingDeviceContext> options)
    : base(options)
    {
    }

    public DbSet<Location> Locations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().ToTable("Tracking_Location");
		modelBuilder.Entity<User>().ToTable("Tracking_User");
        modelBuilder.Entity<Device>().ToTable("Tracking_Device");

        // Device has optional User
        modelBuilder.Entity<Device>()
            .HasOne(d => d.User)
            .WithMany(u => u.Devices)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Location has required Device
        modelBuilder.Entity<Location>()
            .HasOne(l => l.Device)
            .WithMany(d => d.Locations)
            .HasForeignKey(l => l.DeviceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}
