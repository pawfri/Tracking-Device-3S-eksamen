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
	private int _nextId = 4;

    public TrackingDeviceRepo()
    {
        _locations.Add(new Location(1, 10.0000, 20.0000, DateTime.Parse("2025-01-12T11:15:00"), "Automatisk", "Maglegårdsvej 2, 4000 Roskilde, Denmark"));
        _locations.Add(new Location(2 ,10.0000, 20.0000, DateTime.Parse("2025-01-12T12:16:00"), "Automatisk", "Maglegårdsvej 2, 4000 Roskilde, Denmark"));
        _locations.Add(new Location(3, 10.0000, 20.0000, DateTime.Parse("2025-01-12T13:17:00"), "Automatisk", "Maglegårdsvej 2, 4000 Roskilde, Denmark"));
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

    public void Delete(int id)
    {
		// TO DO
    }
}
