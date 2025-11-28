using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingDeviceLib.Services.Repositories;

public class TrackingDeviceRepo
{
	private readonly List<Location> _locations = new();
	private int _nextId = 1;

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
