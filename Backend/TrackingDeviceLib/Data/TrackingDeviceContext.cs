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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().ToTable("Tracking_Location");
		modelBuilder.Entity<Location>().ToTable("Tracking_User");
	}
}
