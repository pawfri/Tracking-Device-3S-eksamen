using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingDeviceLib.Models;
using TrackingDeviceLib.Services.Interfaces;

namespace TrackingDeviceLib.Services.Repositories;

public class TrackingDeviceRepo : ITrackingDeviceRepo
{
	private readonly List<Location> _locations = new();
	private int _nextId = 1;

    public TrackingDeviceRepo()
    {
        _locations.Add(new Location(10.0000, 20.0000, DateTime.Parse("2025-01-12T11:15:00")));
        _locations.Add(new Location(10.0000, 20.0000, DateTime.Parse("2025-01-12T12:16:00")));
        _locations.Add(new Location(10.0000, 20.0000, DateTime.Parse("2025-01-12T13:17:00")));
    }

    public List<Location> GetAll()
	{
		return _locations;
	}

	public Location? GetById(int id)
	{
		return _locations.FirstOrDefault(l => l.Id == id);
	}

	public Location Add(Location location)
	{
		location.Id = _nextId++;
		_locations.Add(location);
		return location;
	}
}
