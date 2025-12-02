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

    public DbSet<Location> locations { get; set; }
}
