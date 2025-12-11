using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingDeviceLib.Data;
using TrackingDeviceLib.Models;
using TrackingDeviceLib.Services.Interfaces;

namespace TrackingDeviceLib.Services.Repositories;

public class LocationDBRepo : ILocationRepo
{
    private readonly TrackingDeviceContext _context;

    public LocationDBRepo(TrackingDeviceContext context)
    {
        _context = context;
    }

    public Location Add(Location location)
    {
        _context.Locations.Add(location);
        _context.SaveChanges();
        return location;
    }

    public List<Location> GetAll()
    {
        return _context.Locations.ToList();
    }

    public Location? GetById(int id)
    {
        return _context.Locations.Find(id);
    }

    public void Delete(int id)
    {
        var location = _context.Locations.Find(id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            _context.SaveChanges();
        }
    }
}
